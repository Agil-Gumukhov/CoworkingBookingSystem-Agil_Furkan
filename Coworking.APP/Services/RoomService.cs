using Coworking.APP.Domain;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Services
{
    public class RoomService : Service<Room>
    {
        public RoomService(CoworkingDb db) : base(db)
        {
        }

        public async Task<List<Room>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbSet().Include(r => r.Branch).ToListAsync(cancellationToken);
        }

        public async Task<Room> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet().Include(r => r.Branch).FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<bool> CreateAsync(Room room, CancellationToken cancellationToken)
        {
            try
            {
                await base.CreateAsync(room, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Room room, CancellationToken cancellationToken)
        {
            try
            {
                await base.UpdateAsync(room, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Room room, CancellationToken cancellationToken)
        {
            try
            {
                await base.DeleteAsync(room, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> HasRelatedDataAsync(int id, CancellationToken cancellationToken)
        {
            var room = await DbSet().Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            return room != null && room.Bookings.Any();
        }
    }
}
