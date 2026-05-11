using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchDeleteHandler : IRequestHandler<BranchDeleteRequest, BranchDeleteResponse>
    {
        private readonly BranchService _service;

        public BranchDeleteHandler(BranchService service)
        {
            _service = service;
        }

        public async Task<BranchDeleteResponse> Handle(BranchDeleteRequest request, CancellationToken cancellationToken)
        {
            var branch = await _service.GetBranchByIdAsync(request.Id, cancellationToken);

            if (branch == null)
                throw new Exception($"Branch with Id {request.Id} not found");

            await _service.DeleteBranchAsync(branch, cancellationToken);

            return new BranchDeleteResponse
            {
                Success = true,
                Message = "Branch deleted successfully"
            };
        }
    }
}
