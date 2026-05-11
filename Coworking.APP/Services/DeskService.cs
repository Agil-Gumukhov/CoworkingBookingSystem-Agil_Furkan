using CORE.APP.Services;
using Coworking.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Services
{
    public class DeskService : Service<Desk>
    {
        public DeskService(CoworkingDb db) : base(db)
        {
        }

        public async Task<List<Desk>> GetAllDesksAsync(CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(d => d.Branch)
                .ToListAsync(cancellationToken);
        }

        public async Task<Desk> GetDeskByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(d => d.Branch)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<Desk> CreateDeskAsync(Desk desk, CancellationToken cancellationToken)
        {
            var branch = await DbSet<Branch>().FirstOrDefaultAsync(b => b.Id == desk.BranchId, cancellationToken);
            if (branch == null)
                throw new InvalidOperationException($"Branch with Id {desk.BranchId} not found");

            await CreateAsync(desk, cancellationToken);
            return desk;
        }

        public async Task<Desk> UpdateDeskAsync(Desk desk, CancellationToken cancellationToken)
        {
            var branch = await DbSet<Branch>().FirstOrDefaultAsync(b => b.Id == desk.BranchId, cancellationToken);
            if (branch == null)
                throw new InvalidOperationException($"Branch with Id {desk.BranchId} not found");

            await UpdateAsync(desk, cancellationToken);
            return desk;
        }

        public async Task DeleteDeskAsync(Desk desk, CancellationToken cancellationToken)
        {
            var deskWithBookings = await DbSet()
                .Include(d => d.Bookings)
                .FirstOrDefaultAsync(d => d.Id == desk.Id, cancellationToken);

            if (deskWithBookings != null && deskWithBookings.Bookings.Any())
                throw new InvalidOperationException("Cannot delete desk that has associated bookings");

            await DeleteAsync(desk, cancellationToken);
        }
    }
}
