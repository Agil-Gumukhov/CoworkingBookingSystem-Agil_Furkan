using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Branches
{
    public class BranchDeleteHandler : IRequestHandler<BranchDeleteRequest, BranchDeleteResponse>
    {
        private readonly CoworkingDb _db;

        public BranchDeleteHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<BranchDeleteResponse> Handle(BranchDeleteRequest request, CancellationToken cancellationToken)
        {
            var branch = await _db.Branches
                .Include(b => b.Rooms)
                .Include(b => b.Desks)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (branch == null)
                throw new Exception($"Branch with Id {request.Id} not found");

            if (branch.Rooms.Any() || branch.Desks.Any())
                throw new Exception("Cannot delete branch that has associated rooms or desks");

            _db.Branches.Remove(branch);
            await _db.SaveChangesAsync(cancellationToken);

            return new BranchDeleteResponse
            {
                Success = true,
                Message = "Branch deleted successfully"
            };
        }
    }
}
