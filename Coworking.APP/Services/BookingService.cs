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
            await ValidateBookingAsync(booking, cancellationToken);
            await CreateAsync(booking, cancellationToken);
            return booking;
        }

        public async Task<Booking> UpdateBookingAsync(Booking booking, CancellationToken cancellationToken)
        {
            await ValidateBookingAsync(booking, cancellationToken);
            await UpdateAsync(booking, cancellationToken);
            return booking;
        }

        public async Task DeleteBookingAsync(Booking booking, CancellationToken cancellationToken)
        {
            await DeleteAsync(booking, cancellationToken);
        }

        private async Task ValidateBookingAsync(Booking booking, CancellationToken cancellationToken)
        {
            if (booking.EndDate <= booking.StartDate)
                throw new InvalidOperationException("End date must be later than start date");

            if (booking.RoomId.HasValue == booking.DeskId.HasValue)
                throw new InvalidOperationException("Select either one room or one desk for a booking");

            var branchExists = await DbSet<Branch>()
                .AnyAsync(b => b.Id == booking.BranchId, cancellationToken);
            if (!branchExists)
                throw new InvalidOperationException($"Branch with Id {booking.BranchId} not found");

            if (booking.RoomId.HasValue)
            {
                var room = await DbSet<Room>()
                    .FirstOrDefaultAsync(r => r.Id == booking.RoomId.Value, cancellationToken);
                if (room is null)
                    throw new InvalidOperationException($"Room with Id {booking.RoomId} not found");
                if (room.BranchId != booking.BranchId)
                    throw new InvalidOperationException("Selected room does not belong to the selected branch");

                var hasConflict = await DbSet()
                    .AnyAsync(b => b.Id != booking.Id
                        && b.RoomId == booking.RoomId
                        && b.Status != "Cancelled"
                        && booking.StartDate < b.EndDate
                        && booking.EndDate > b.StartDate, cancellationToken);
                if (hasConflict)
                    throw new InvalidOperationException("Selected room already has a booking in this time interval");
            }

            if (booking.DeskId.HasValue)
            {
                var desk = await DbSet<Desk>()
                    .FirstOrDefaultAsync(d => d.Id == booking.DeskId.Value, cancellationToken);
                if (desk is null)
                    throw new InvalidOperationException($"Desk with Id {booking.DeskId} not found");
                if (desk.BranchId != booking.BranchId)
                    throw new InvalidOperationException("Selected desk does not belong to the selected branch");

                var hasConflict = await DbSet()
                    .AnyAsync(b => b.Id != booking.Id
                        && b.DeskId == booking.DeskId
                        && b.Status != "Cancelled"
                        && booking.StartDate < b.EndDate
                        && booking.EndDate > b.StartDate, cancellationToken);
                if (hasConflict)
                    throw new InvalidOperationException("Selected desk already has a booking in this time interval");
            }
        }
    }
}
