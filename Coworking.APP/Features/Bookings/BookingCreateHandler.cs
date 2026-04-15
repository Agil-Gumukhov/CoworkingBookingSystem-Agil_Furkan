using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Bookings
{
    public class BookingCreateHandler : IRequestHandler<BookingCreateRequest, BookingCreateResponse>
    {
        private readonly CoworkingDb _db;

        public BookingCreateHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<BookingCreateResponse> Handle(BookingCreateRequest request, CancellationToken cancellationToken)
        {
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

            var booking = new Booking
            {
                UserId = request.UserId,
                BranchId = request.BranchId,
                RoomId = request.RoomId,
                DeskId = request.DeskId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync(cancellationToken);

            return new BookingCreateResponse
            {
                Id = booking.Id,
                UserId = booking.UserId,
                BranchId = booking.BranchId,
                RoomId = booking.RoomId,
                DeskId = booking.DeskId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status,
                Message = "Booking created successfully"
            };
        }
    }
}
