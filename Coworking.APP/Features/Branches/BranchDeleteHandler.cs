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
            var branch = await _service.GetByIdAsync(request.Id, cancellationToken);

            if (branch == null)
                throw new Exception($"Branch with Id {request.Id} not found");

            var hasRelatedData = await _service.HasRelatedDataAsync(request.Id, cancellationToken);
            if (hasRelatedData)
                throw new Exception("Cannot delete branch that has associated rooms or desks");

            var success = await _service.DeleteAsync(branch, cancellationToken);

            if (!success)
                throw new Exception("Failed to delete branch");

            return new BranchDeleteResponse
            {
                Success = true,
                Message = "Branch deleted successfully"
            };
        }
    }
}
