using System.Net;
using BuildingBlocks.Core.Domain.Exceptions;

namespace Ytsoob.Modules.Posts.Contents.Exceptions.Domain;

public class FileNotFound : DomainException
{
    public FileNotFound(string url, HttpStatusCode statusCode = HttpStatusCode.NotFound) : base($"File with url {url} not found", statusCode)
    {
    }
}
