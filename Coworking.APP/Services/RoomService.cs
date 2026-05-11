using CORE.APP.Services;
using Coworking.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Services
{
    public class RoomService : Service<Room>
    {
        public RoomService(CoworkingDb db) : base(db)
        {
        }

        public async Task<List<Room>> GetAllRoomsAsync(CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(r => r.Branch)
                .ToListAsync(cancellationToken);
        }

        public async Task<Room> GetRoomByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(r => r.Branch)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<Room> CreateRoomAsync(Room room, CancellationToken cancellationToken)
        {
            var branch = await DbSet<Branch>().FirstOrDefaultAsync(b => b.Id == room.BranchId, cancellationToken);
            if (branch == null)
                throw new InvalidOperationException($"Branch with Id {room.BranchId} not found");

            await CreateAsync(room, cancellationToken);
            return room;
        }

        public async Task<Room> UpdateRoomAsync(Room room, CancellationToken cancellationToken)
        {
            var branch = await DbSet<Branch>().FirstOrDefaultAsync(b => b.Id == room.BranchId, cancellationToken);
            if (branch == null)
                throw new InvalidOperationException($"Branch with Id {room.BranchId} not found");

            await UpdateAsync(room, cancellationToken);
            return room;
        }

        public async Task DeleteRoomAsync(Room room, CancellationToken cancellationToken)
        {
            var roomWithBookings = await DbSet()
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == room.Id, cancellationToken);

            if (roomWithBookings != null && roomWithBookings.Bookings.Any())
                throw new InvalidOperationException("Cannot delete room that has associated bookings");

            await DeleteAsync(room, cancellationToken);
        }
    }
}
