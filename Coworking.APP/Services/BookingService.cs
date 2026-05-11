using CORE.APP.Services;
using Coworking.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Services
{
    public class BookingService : Service<Booking>
    {
        public BookingService(CoworkingDb db) : base(db)
        {
        }

        public async Task<List<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(b => b.Branch)
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .ToListAsync(cancellationToken);
        }

        public async Task<Booking> GetBookingByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(b => b.Branch)
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<Booking> CreateBookingAsync(Booking booking, CancellationToken cancellationToken)
        {
            var branch = await DbSet<Branch>().FirstOrDefaultAsync(b => b.Id == booking.BranchId, cancellationToken);
            if (branch == null)
                throw new InvalidOperationException($"Branch with Id {booking.BranchId} not found");

            if (booking.RoomId.HasValue)
            {
                var room = await DbSet<Room>().FirstOrDefaultAsync(r => r.Id == booking.RoomId, cancellationToken);
                if (room == null)
                    throw new InvalidOperationException($"Room with Id {booking.RoomId} not found");
            }

            if (booking.DeskId.HasValue)
            {
                var desk = await DbSet<Desk>().FirstOrDefaultAsync(d => d.Id == booking.DeskId, cancellationToken);
                if (desk == null)
                    throw new InvalidOperationException($"Desk with Id {booking.DeskId} not found");
            }

            await CreateAsync(booking, cancellationToken);
            return booking;
        }

        public async Task<Booking> UpdateBookingAsync(Booking booking, CancellationToken cancellationToken)
        {
            var branch = await DbSet<Branch>().FirstOrDefaultAsync(b => b.Id == booking.BranchId, cancellationToken);
            if (branch == null)
                throw new InvalidOperationException($"Branch with Id {booking.BranchId} not found");

            if (booking.RoomId.HasValue)
            {
                var room = await DbSet<Room>().FirstOrDefaultAsync(r => r.Id == booking.RoomId, cancellationToken);
                if (room == null)
                    throw new InvalidOperationException($"Room with Id {booking.RoomId} not found");
            }

            if (booking.DeskId.HasValue)
            {
                var desk = await DbSet<Desk>().FirstOrDefaultAsync(d => d.Id == booking.DeskId, cancellationToken);
                if (desk == null)
                    throw new InvalidOperationException($"Desk with Id {booking.DeskId} not found");
            }

            await UpdateAsync(booking, cancellationToken);
            return booking;
        }

        public async Task DeleteBookingAsync(Booking booking, CancellationToken cancellationToken)
        {
            await DeleteAsync(booking, cancellationToken);
        }
    }
}
