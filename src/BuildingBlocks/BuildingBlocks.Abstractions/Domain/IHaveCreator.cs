namespace BuildingBlocks.Abstractions.Domain;

public interface IHaveCreator
{
    DateTime Created { get; }
    long? CreatedBy { get; }
}
