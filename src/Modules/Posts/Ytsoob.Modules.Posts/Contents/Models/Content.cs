using BuildingBlocks.Core.Domain;
using Ytsoob.Modules.Posts.Contents.Exceptions.Domain;
using Ytsoob.Modules.Posts.Contents.ValueObjects;
using Ytsoob.Modules.Posts.Posts.Exception;
using Ytsoob.Modules.Posts.Posts.ValueObjects;

namespace Ytsoob.Modules.Posts.Contents.Models;

public class Content : Entity<ContentId>
{
    public ContentText ContentText { get; private set; } = default!;
    public PostId PostId { get; private set; } = default!;
    public List<string> Files { get; set; } = new();
    public Content(ContentId contentId, ContentText contentText)
    {
        Id = contentId;
        UpdateText(contentText);
    }

    // ef core
    protected Content()
    {
    }

    public void UpdateText(ContentText text)
    {
        ContentText = text;
    }

    public void AddFile(string fileUrl)
    {
        Files.Add(fileUrl);
    }

    public void RemoveFile(string fileUrl)
    {
        if (!Files.Contains(fileUrl))
        {
            throw new FileNotFound(fileUrl);
        }

        Files.Remove(fileUrl);
    }
}
