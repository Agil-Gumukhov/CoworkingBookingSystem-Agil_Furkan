using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Bookings
{
    public class BookingDeleteHandler : IRequestHandler<BookingDeleteRequest, BookingDeleteResponse>
    {
        private readonly CoworkingDb _db;

        public BookingDeleteHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<BookingDeleteResponse> Handle(BookingDeleteRequest request, CancellationToken cancellationToken)
        {
            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (booking == null)
                throw new Exception($"Booking with Id {request.Id} not found");

            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync(cancellationToken);

            return new BookingDeleteResponse
            {
                Success = true,
                Message = "Booking deleted successfully"
            };
        }
    }
}
