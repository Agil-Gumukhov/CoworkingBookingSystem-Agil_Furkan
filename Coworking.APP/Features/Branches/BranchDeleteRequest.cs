using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchDeleteRequest : IRequest<BranchDeleteResponse>
    {
        public int Id { get; set; }
    }
}
