using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Bookings
{
    public class BookingCreateHandler : IRequestHandler<BookingCreateRequest, BookingCreateResponse>
    {
        private readonly BookingService _service;

        public BookingCreateHandler(BookingService service)
        {
            _service = service;
        }

        public async Task<BookingCreateResponse> Handle(BookingCreateRequest request, CancellationToken cancellationToken)
        {
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

            booking = await _service.CreateBookingAsync(booking, cancellationToken);

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
