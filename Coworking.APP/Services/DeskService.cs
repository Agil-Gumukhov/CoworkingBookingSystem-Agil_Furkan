using Coworking.APP.Domain;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Services
{
    public class DeskService : Service<Desk>
    {
        public DeskService(CoworkingDb db) : base(db)
        {
        }

        public async Task<List<Desk>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbSet().Include(d => d.Branch).ToListAsync(cancellationToken);
        }

        public async Task<Desk> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet().Include(d => d.Branch).FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<bool> CreateAsync(Desk desk, CancellationToken cancellationToken)
        {
            try
            {
                await base.CreateAsync(desk, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Desk desk, CancellationToken cancellationToken)
        {
            try
            {
                await base.UpdateAsync(desk, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Desk desk, CancellationToken cancellationToken)
        {
            try
            {
                await base.DeleteAsync(desk, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> HasRelatedDataAsync(int id, CancellationToken cancellationToken)
        {
            var desk = await DbSet().Include(d => d.Bookings)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
            return desk != null && desk.Bookings.Any();
        }
    }
}
