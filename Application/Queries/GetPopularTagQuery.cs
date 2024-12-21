using Application.Models.Result;
using MediatR;

namespace Application.Queries;
public class GetPopularTagQuery : IRequest<TagListResult>
{
    public GetPopularTagQuery()
    {
    }
}
