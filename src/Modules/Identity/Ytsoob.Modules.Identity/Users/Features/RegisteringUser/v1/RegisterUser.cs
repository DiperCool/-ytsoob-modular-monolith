using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Core.IdsGenerator;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Ytsoob.Modules.Identity.Shared.Models;
using Ytsoob.Modules.Identity.Users.Dtos.v1;
using Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1.Events.Integration;
using UserState = Ytsoob.Modules.Identity.Shared.Models.UserState;

namespace Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1;

public record RegisterUser(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    string Phone,
    string ConfirmPassword,
    List<string>? Roles = null
) : ITxCreateCommand<RegisterUserResponse>
{
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}

internal class RegisterUserValidator : AbstractValidator<RegisterUser>
{
    public RegisterUserValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(v => v.FirstName).NotEmpty().WithMessage("FirstName is required.");

        RuleFor(v => v.LastName).NotEmpty().WithMessage("LastName is required.");

        RuleFor(v => v.Email).NotEmpty().WithMessage("Email is required.").EmailAddress();

        RuleFor(v => v.UserName).NotEmpty().WithMessage("UserName is required.");
        RuleFor(v => v.Phone).NotEmpty().WithMessage("Phone is required.");

        RuleFor(v => v.Password).NotEmpty().WithMessage("Password is required.");

        RuleFor(v => v.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("The password and confirmation password do not match.")
            .NotEmpty();

        RuleFor(v => v.Roles)
            .Custom(
                (roles, c) =>
                {
                    if (
                        roles != null
                        && !roles.All(x =>
                            x.Contains(IdentityConstants.Role.Admin, StringComparison.Ordinal)
                            || x.Contains(IdentityConstants.Role.User, StringComparison.Ordinal)
                        )
                    )
                    {
                        c.AddFailure("Invalid roles.");
                    }
                }
            );
    }
}

// using transaction script instead of using domain business logic here
// https://www.youtube.com/watch?v=PrJIMTZsbDw
internal class RegisterUserHandler : ICommandHandler<RegisterUser, RegisterUserResponse>
{
    private readonly IMessagePersistenceService _messagePersistenceService;

    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterUserHandler(
        UserManager<ApplicationUser> userManager,
        IMessagePersistenceService messagePersistenceService
    )
    {
        _messagePersistenceService = messagePersistenceService;
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
    }

    public async Task<RegisterUserResponse> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            UserState = UserState.Active,
            CreatedAt = request.CreatedAt,
            PhoneNumber = request.Phone,
            YtsooberId = SnowFlakIdGenerator.NewId()
        };

        var identityResult = await _userManager.CreateAsync(applicationUser, request.Password);
        if (!identityResult.Succeeded)
            throw new RegisterIdentityUserException(string.Join(',', identityResult.Errors.Select(e => e.Description)));

        var roleResult = await _userManager.AddToRolesAsync(
            applicationUser,
            request.Roles ?? new List<string> { IdentityConstants.Role.User }
        );

        if (!roleResult.Succeeded)
            throw new RegisterIdentityUserException(string.Join(',', roleResult.Errors.Select(e => e.Description)));

        var userRegistered = new UserRegistered(
            applicationUser.Id,
            applicationUser.YtsooberId,
            applicationUser.UserName,
            applicationUser.Email,
            applicationUser.PhoneNumber!,
            request.Roles
        );

        // publish our integration event and save to outbox should do in same transaction of our business logic actions. we could use TxBehaviour or ITxDbContextExecutes interface
        // This service is not DDD, so we couldn't use DomainEventPublisher to publish mapped integration events
        await _messagePersistenceService.AddPublishMessageAsync(
            new MessageEnvelope<UserRegistered>(userRegistered, new Dictionary<string, string>()),
            cancellationToken
        );
        return new RegisterUserResponse(
            new IdentityUserDto
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email,
                PhoneNumber = applicationUser.PhoneNumber,
                UserName = applicationUser.UserName,
                Roles = request.Roles ?? new List<string> { IdentityConstants.Role.User },
                RefreshTokens = applicationUser?.RefreshTokens?.Select(x => x.Token),
                CreatedAt = request.CreatedAt,
                UserState = UserState.Active
            }
        );
    }
}
