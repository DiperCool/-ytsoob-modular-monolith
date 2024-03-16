using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ytsoob.Modules.Ytsoobers.Profiles.ValueObjects;
using Ytsoob.Modules.Ytsoobers.Shared.Contracts;
using Ytsoob.Modules.Ytsoobers.Shared.Data;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Exceptions;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Models;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Modules.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;

public record UpdateProfile(string FirstName, string LastName, IFormFile? Avatar, YtsooberId YtsooberId)
    : ITxUpdateCommand<Unit>;

public class UpdateProfileValidator : AbstractValidator<UpdateProfile>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(15);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(15);
    }
}

public class UpdateProfileHandler : ICommandHandler<UpdateProfile, Unit>
{
    private YtsoobersDbContext _ytsoobersDbContext;
    private ICurrentUserService _currentUserService;
    private ILogger<UpdateProfileHandler> _logger;
    private IAvatarStorage _avatarStorage;

    public UpdateProfileHandler(
        YtsoobersDbContext ytsoobersDbContext,
        ICurrentUserService currentUserService,
        ILogger<UpdateProfileHandler> logger,
        IAvatarStorage avatarStorage
    )
    {
        _ytsoobersDbContext = ytsoobersDbContext;
        _currentUserService = currentUserService;
        _logger = logger;
        _avatarStorage = avatarStorage;
    }

    public async Task<Unit> Handle(UpdateProfile request, CancellationToken cancellationToken)
    {
        Ytsoober? ytsoober = await _ytsoobersDbContext
            .Ytsoobers.Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.Id == _currentUserService.YtsooberId, cancellationToken: cancellationToken);
        if (ytsoober == null)
        {
            _logger.LogCritical("Ytsoober with Id = {YtsooberId} not found", _currentUserService.YtsooberId);
            throw new YtsooberNotFoundException(_currentUserService.YtsooberId);
        }

        string? avatar =
            request.Avatar != null ? await _avatarStorage.UploadAvatarAsync(request.Avatar, cancellationToken) : null;
        ytsoober.UpdateProfile(FirstName.Of(request.FirstName), LastName.Of(request.LastName), avatar);
        _ytsoobersDbContext.Ytsoobers.Update(ytsoober);
        await _ytsoobersDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
