using Coworking.APP.Domain;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchCreateHandler : IRequestHandler<BranchCreateRequest, BranchCreateResponse>
    {
        private readonly CoworkingDb _db;

        public BranchCreateHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<BranchCreateResponse> Handle(BranchCreateRequest request, CancellationToken cancellationToken)
        {
            var branch = new Branch
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City
            };

            _db.Branches.Add(branch);
            await _db.SaveChangesAsync(cancellationToken);

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
