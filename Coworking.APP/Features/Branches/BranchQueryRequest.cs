using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchQueryRequest : IRequest<BranchQueryResponse>
    {
        public int Id { get; set; }
    }
}
