using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Posts.Contents.Exceptions.Domain;

public class NotEmptyTextException : AppException
{
    public NotEmptyTextException(string text)
        : base($"Content Text: {text} Is Invalid.")
    {
    }
}
