using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Branches
{
    public class BranchUpdateHandler : IRequestHandler<BranchUpdateRequest, BranchUpdateResponse>
    {
        private readonly CoworkingDb _db;

        public BranchUpdateHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<BranchUpdateResponse> Handle(BranchUpdateRequest request, CancellationToken cancellationToken)
        {
            var branch = await _db.Branches.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
            if (branch == null)
                throw new Exception($"Branch with Id {request.Id} not found");

            branch.Name = request.Name;
            branch.Address = request.Address;
            branch.City = request.City;

            _db.Branches.Update(branch);
            await _db.SaveChangesAsync(cancellationToken);

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
