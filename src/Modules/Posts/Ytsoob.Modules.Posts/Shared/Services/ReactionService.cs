using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Core.IdsGenerator;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Reactions.Enums;
using Ytsoob.Modules.Posts.Reactions.Models;
using Ytsoob.Modules.Posts.Reactions.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Contracts;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Shared.Services;

public class ReactionService : IReactionService
{
    private PostsDbContext _postsDbContext;

    public ReactionService(PostsDbContext postsDbContext)
    {
        _postsDbContext = postsDbContext;
    }

    private async Task<bool> YtsooberSetReaction(long ytsooberId, string entityId, string entityType)
    {
        return await _postsDbContext.Reactions.AnyAsync(x =>
            x.YtsooberId == ytsooberId && x.EntityId == entityId && x.EntityType == entityType
        );
    }

    public async Task AddReactionAsync<T, TId>(
        TId entityId,
        long ytsooberId,
        ReactionType reactionType,
        CancellationToken cancellationToken = default
    )
        where T : class, IEntityWithReactions<TId>
    {
        if (await YtsooberSetReaction(ytsooberId, entityId!.ToString(), typeof(T).ToString()))
        {
            throw new BadRequestException($"Ytsoober already set reaction to this entity with Id = {entityId}");
        }

        T? entity = await _postsDbContext
            .Set<T>()
            .Include(x => x.ReactionStats)
            .FirstOrDefaultAsync(x => x.Id!.Equals(entityId), cancellationToken: cancellationToken);
        if (entity == null)
        {
            throw new BadRequestException($"Entity with Id = {entityId} not found");
        }

        Reaction reaction = Reaction.Create<T, TId>(
            ReactionId.Of(SnowFlakIdGenerator.NewId()),
            entityId,
            typeof(T).ToString(),
            ytsooberId,
            reactionType
        );
        await _postsDbContext.Reactions.AddAsync(reaction, cancellationToken);
        entity.AddReaction(reactionType);
        _postsDbContext.Set<T>().Update(entity);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveReactionAsync<T, TId>(
        TId entityId,
        long ytsooberId,
        CancellationToken cancellationToken = default
    )
        where T : class, IEntityWithReactions<TId>
    {
        if (!await YtsooberSetReaction(ytsooberId, entityId!.ToString(), typeof(T).ToString()))
        {
            throw new BadRequestException($"Ytsoober doesn't set reaction to this entity with Id = {entityId}");
        }

        Reaction? reaction = await _postsDbContext.Reactions.FirstOrDefaultAsync(
            x =>
                x.YtsooberId == ytsooberId && x.EntityId == entityId.ToString() && x.EntityType == typeof(T).ToString(),
            cancellationToken: cancellationToken
        );
        if (reaction == null)
        {
            throw new BadRequestException($"Reaction not found in entity with Id = {entityId}");
        }
        T? entity = await _postsDbContext
            .Set<T>()
            .Include(x => x.ReactionStats)
            .FirstOrDefaultAsync(x => x.Id!.Equals(entityId), cancellationToken: cancellationToken);
        if (entity == null)
        {
            throw new BadRequestException($"Entity with Id = {entityId} not found");
        }

        entity.RemoveReaction(reaction.ReactionType);
        _postsDbContext.Set<T>().Update(entity);

        _postsDbContext.Reactions.Remove(reaction);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
    }
}
