using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Bookings
{
    public class BookingUpdateHandler : IRequestHandler<BookingUpdateRequest, BookingUpdateResponse>
    {
        private readonly CoworkingDb _db;

        public BookingUpdateHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<BookingUpdateResponse> Handle(BookingUpdateRequest request, CancellationToken cancellationToken)
        {
            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
            if (booking == null)
                throw new Exception($"Booking with Id {request.Id} not found");

            var branchExists = await _db.Branches.AnyAsync(b => b.Id == request.BranchId, cancellationToken);
            if (!branchExists)
                throw new Exception($"Branch with Id {request.BranchId} not found");

            if (request.RoomId.HasValue)
            {
                var roomExists = await _db.Rooms.AnyAsync(r => r.Id == request.RoomId, cancellationToken);
                if (!roomExists)
                    throw new Exception($"Room with Id {request.RoomId} not found");
            }

            if (request.DeskId.HasValue)
            {
                var deskExists = await _db.Desks.AnyAsync(d => d.Id == request.DeskId, cancellationToken);
                if (!deskExists)
                    throw new Exception($"Desk with Id {request.DeskId} not found");
            }

            booking.UserId = request.UserId;
            booking.BranchId = request.BranchId;
            booking.RoomId = request.RoomId;
            booking.DeskId = request.DeskId;
            booking.StartDate = request.StartDate;
            booking.EndDate = request.EndDate;
            booking.Status = request.Status;

            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync(cancellationToken);

            return new BookingUpdateResponse
            {
                Id = booking.Id,
                UserId = booking.UserId,
                BranchId = booking.BranchId,
                RoomId = booking.RoomId,
                DeskId = booking.DeskId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status,
                Message = "Booking updated successfully"
            };
        }
    }
}
