namespace BuildingBlocks.Abstractions.CQRS.Query;

public interface IListQuery<out TResponse> : IQuery<TResponse>, IPageRequest
    where TResponse : notnull;
