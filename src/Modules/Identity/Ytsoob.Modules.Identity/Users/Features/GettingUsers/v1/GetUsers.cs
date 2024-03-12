using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.CQRS.Query;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Ytsoob.Modules.Identity.Shared.Extensions;
using Ytsoob.Modules.Identity.Shared.Models;
using Ytsoob.Modules.Identity.Users.Dtos.v1;

namespace Ytsoob.Modules.Identity.Users.Features.GettingUsers.v1;

public record GetUsers : ListQuery<GetUsersResponse>;

public class GetUsersValidator : AbstractValidator<GetUsers>
{
    public GetUsersValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetUsersHandler : IQueryHandler<GetUsers, GetUsersResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public GetUsersHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<GetUsersResponse> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        var customer = await _userManager.FindAllUsersByPageAsync<IdentityUserDto>(_mapper, request, cancellationToken);

        return new GetUsersResponse(customer);
    }
}
