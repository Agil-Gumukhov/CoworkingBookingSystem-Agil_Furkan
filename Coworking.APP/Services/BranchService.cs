using CORE.APP.Services;
using Coworking.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Services
{
    public class BranchService : Service<Branch>
    {
        public BranchService(CoworkingDb db) : base(db)
        {
        }

        public async Task<List<Branch>> GetAllBranchesAsync(CancellationToken cancellationToken)
        {
            return await DbSet().ToListAsync(cancellationToken);
        }

        public async Task<Branch> GetBranchByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<Branch> CreateBranchAsync(Branch branch, CancellationToken cancellationToken)
        {
            await CreateAsync(branch, cancellationToken);
            return branch;
        }

        public async Task<Branch> UpdateBranchAsync(Branch branch, CancellationToken cancellationToken)
        {
            await UpdateAsync(branch, cancellationToken);
            return branch;
        }

        public async Task DeleteBranchAsync(Branch branch, CancellationToken cancellationToken)
        {
            var branchWithRoomsDesks = await DbSet()
                .Include(b => b.Rooms)
                .Include(b => b.Desks)
                .FirstOrDefaultAsync(b => b.Id == branch.Id, cancellationToken);

            if (branchWithRoomsDesks != null && (branchWithRoomsDesks.Rooms.Any() || branchWithRoomsDesks.Desks.Any()))
                throw new InvalidOperationException("Cannot delete branch that has associated rooms or desks");

            await DeleteAsync(branch, cancellationToken);
        }
    }
}
