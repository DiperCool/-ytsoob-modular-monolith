namespace BuildingBlocks.BlobStorage.Policies;

public interface IBucketPolicy
{
    string Get(string bucket);
}
