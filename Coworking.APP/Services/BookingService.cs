using Coworking.APP.Domain;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Services
{
    public class BookingService : Service<Booking>
    {
        public BookingService(CoworkingDb db) : base(db)
        {
        }

        public async Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(b => b.Branch)
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .ToListAsync(cancellationToken);
        }

        public async Task<Booking> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(b => b.Branch)
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<bool> CreateAsync(Booking booking, CancellationToken cancellationToken)
        {
            try
            {
                await base.CreateAsync(booking, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Booking booking, CancellationToken cancellationToken)
        {
            try
            {
                await base.UpdateAsync(booking, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Booking booking, CancellationToken cancellationToken)
        {
            try
            {
                await base.DeleteAsync(booking, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
