using AutoMapper;
using Ytsoob.Modules.Posts.Contents.Dtos;
using Ytsoob.Modules.Posts.Contents.Features.UpdatingPostContent.v1;
using Ytsoob.Modules.Posts.Contents.Models;
using Ytsoob.Modules.Posts.Polls.Dtos;
using Ytsoob.Modules.Posts.Posts.Dtos;
using Ytsoob.Modules.Posts.Posts.Features.CreatingPost.v1;
using Ytsoob.Modules.Posts.Posts.Features.DeletingPost;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Reactions;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Posts;

public class PostMapper : Profile
{
    public PostMapper()
    {
        CreateMap<Post, PostDto>().ForMember(x => x.Id, expression => expression.MapFrom(x => x.Id.Value));
        CreateMap<Content, ContentDto>()
            .ForMember(x => x.ContentText, expression => expression.MapFrom(x => x.ContentText.Value));
        CreateMap<CreatePostRequest, CreatePost>().ConstructUsing(req => new CreatePost(req.Content, req.Poll));
        CreateMap<UpdatePostContentRequest, UpdatePostContent>();
        CreateMap<DeletePostRequest, DeletePost>();
        CreateMap<Ytsoober, VoterDto>();
        this.CreateMapReactionStats();
    }
}
