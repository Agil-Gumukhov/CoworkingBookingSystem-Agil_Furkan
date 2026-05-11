using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Bookings
{
    public class BookingUpdateHandler : IRequestHandler<BookingUpdateRequest, BookingUpdateResponse>
    {
        private readonly BookingService _service;

        public BookingUpdateHandler(BookingService service)
        {
            _service = service;
        }

        public async Task<BookingUpdateResponse> Handle(BookingUpdateRequest request, CancellationToken cancellationToken)
        {
            var booking = await _service.GetBookingByIdAsync(request.Id, cancellationToken);
            if (booking == null)
                throw new Exception($"Booking with Id {request.Id} not found");

            booking.UserId = request.UserId;
            booking.BranchId = request.BranchId;
            booking.RoomId = request.RoomId;
            booking.DeskId = request.DeskId;
            booking.StartDate = request.StartDate;
            booking.EndDate = request.EndDate;
            booking.Status = request.Status;

            booking = await _service.UpdateBookingAsync(booking, cancellationToken);

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
