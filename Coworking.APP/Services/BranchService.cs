using Coworking.APP.Domain;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Services
{
    public class BranchService : Service<Branch>
    {
        public BranchService(CoworkingDb db) : base(db)
        {
        }

        public async Task<List<Branch>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbSet().ToListAsync(cancellationToken);
        }

        public async Task<Branch> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<bool> CreateAsync(Branch branch, CancellationToken cancellationToken)
        {
            try
            {
                await base.CreateAsync(branch, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Branch branch, CancellationToken cancellationToken)
        {
            try
            {
                await base.UpdateAsync(branch, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Branch branch, CancellationToken cancellationToken)
        {
            try
            {
                await base.DeleteAsync(branch, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> HasRelatedDataAsync(int id, CancellationToken cancellationToken)
        {
            var branch = await DbSet().Include(b => b.Rooms).Include(b => b.Desks)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            return branch != null && (branch.Rooms.Any() || branch.Desks.Any());
        }
    }
}
