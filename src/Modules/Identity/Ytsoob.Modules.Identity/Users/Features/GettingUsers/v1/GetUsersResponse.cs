using BuildingBlocks.Core.CQRS.Query;
using Ytsoob.Modules.Identity.Users.Dtos.v1;

namespace Ytsoob.Modules.Identity.Users.Features.GettingUsers.v1;

public record GetUsersResponse(ListResultModel<IdentityUserDto> IdentityUsers);
