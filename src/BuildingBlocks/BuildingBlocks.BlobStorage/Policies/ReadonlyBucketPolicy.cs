using Newtonsoft.Json;

namespace BuildingBlocks.BlobStorage.Policies;

public class ReadonlyBucketPolicy : IBucketPolicy
{
    public string Get(string bucket)
    {
        return JsonConvert.SerializeObject(new
       {
           Version="2012-10-17",
           Statement = new[] {
             new {
                 Effect = "Allow",
                 Principal = "*",
                 Action = new[] {"s3:GetObject"},
                 Resource = new[] {$"arn:aws:s3:::{bucket}/*"},
                 Sid = ""
             }
         }
       });
    }
}
