using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchCreateHandler : IRequestHandler<BranchCreateRequest, BranchCreateResponse>
    {
        private readonly BranchService _service;

        public BranchCreateHandler(BranchService service)
        {
            _service = service;
        }

        public async Task<BranchCreateResponse> Handle(BranchCreateRequest request, CancellationToken cancellationToken)
        {
            var branch = new Branch
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City
            };

            var success = await _service.CreateAsync(branch, cancellationToken);

            if (!success)
                throw new Exception("Failed to create branch");

            return new BranchCreateResponse
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                City = branch.City,
                Message = "Branch created successfully"
            };
        }
    }
}
