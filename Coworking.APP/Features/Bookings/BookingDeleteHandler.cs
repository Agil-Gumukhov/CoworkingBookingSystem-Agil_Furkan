using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Bookings
{
    public class BookingDeleteHandler : IRequestHandler<BookingDeleteRequest, BookingDeleteResponse>
    {
        private readonly BookingService _service;

        public BookingDeleteHandler(BookingService service)
        {
            _service = service;
        }

        public async Task<BookingDeleteResponse> Handle(BookingDeleteRequest request, CancellationToken cancellationToken)
        {
            var booking = await _service.GetBookingByIdAsync(request.Id, cancellationToken);

            if (booking == null)
                throw new Exception($"Booking with Id {request.Id} not found");

            await _service.DeleteBookingAsync(booking, cancellationToken);

            return new BookingDeleteResponse
            {
                Success = true,
                Message = "Booking deleted successfully"
            };
        }
    }
}
