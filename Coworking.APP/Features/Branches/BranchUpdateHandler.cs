using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchUpdateHandler : IRequestHandler<BranchUpdateRequest, BranchUpdateResponse>
    {
        private readonly BranchService _service;

        public BranchUpdateHandler(BranchService service)
        {
            _service = service;
        }

        public async Task<BranchUpdateResponse> Handle(BranchUpdateRequest request, CancellationToken cancellationToken)
        {
            var branch = await _service.GetBranchByIdAsync(request.Id, cancellationToken);
            if (branch == null)
                throw new Exception($"Branch with Id {request.Id} not found");

            branch.Name = request.Name;
            branch.Address = request.Address;
            branch.City = request.City;

            branch = await _service.UpdateBranchAsync(branch, cancellationToken);

            return new BranchUpdateResponse
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                City = branch.City,
                Message = "Branch updated successfully"
            };
        }
    }
}
