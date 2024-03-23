using AutoMapper;
using Ytsoob.Modules.Posts.Polls.Dtos;
using Ytsoob.Modules.Posts.Polls.Models;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Polls;

public class PollMapper : Profile
{
    public PollMapper()
    {
        CreateMap<Poll, PollDto>()
            .ForMember(x => x.Id, expression => expression.MapFrom(poll => poll.Id.Value))
            .ForMember(x => x.Question, expression => expression.MapFrom(poll => poll.Question.Value));

        CreateMap<Option, OptionDto>()
            .ForMember(x => x.Id, expression => expression.MapFrom(poll => poll.Id.Value))
            .ForMember(x => x.Count, expression => expression.MapFrom(poll => poll.Count.Value))
            .ForMember(x => x.Fiction, expression => expression.MapFrom(poll => poll.Fiction.Value))
            .ForMember(x => x.Title, expression => expression.MapFrom(poll => poll.Title.Value));
        CreateMap<Ytsoober, Voter>();
    }
}
