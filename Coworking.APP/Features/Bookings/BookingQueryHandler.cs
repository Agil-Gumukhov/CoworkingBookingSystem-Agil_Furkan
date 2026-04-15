using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Bookings
{
    public class BookingQueryHandler : IRequestHandler<BookingQueryRequest, BookingQueryResponse>
    {
        private readonly CoworkingDb _db;

        public BookingQueryHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<BookingQueryResponse> Handle(BookingQueryRequest request, CancellationToken cancellationToken)
        {
            var booking = await _db.Bookings
                .Include(b => b.Branch)
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (booking == null)
                throw new Exception($"Booking with Id {request.Id} not found");

            return new BookingQueryResponse
            {
                Id = booking.Id,
                UserId = booking.UserId,
                BranchId = booking.BranchId,
                BranchName = booking.Branch?.Name,
                RoomId = booking.RoomId,
                RoomName = booking.Room?.Name,
                DeskId = booking.DeskId,
                DeskCode = booking.Desk?.Code,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status
            };
        }
    }

    public class BookingQueryAllRequest : IRequest<List<BookingQueryResponse>>
    {
    }

    public class BookingQueryAllHandler : IRequestHandler<BookingQueryAllRequest, List<BookingQueryResponse>>
    {
        private readonly CoworkingDb _db;

        public BookingQueryAllHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<List<BookingQueryResponse>> Handle(BookingQueryAllRequest request, CancellationToken cancellationToken)
        {
            var bookings = await _db.Bookings
                .Include(b => b.Branch)
                .Include(b => b.Room)
                .Include(b => b.Desk)
                .Select(b => new BookingQueryResponse
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    BranchId = b.BranchId,
                    BranchName = b.Branch.Name,
                    RoomId = b.RoomId,
                    RoomName = b.Room.Name,
                    DeskId = b.DeskId,
                    DeskCode = b.Desk.Code,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Status = b.Status
                })
                .ToListAsync(cancellationToken);

            return bookings;
        }
    }
}
