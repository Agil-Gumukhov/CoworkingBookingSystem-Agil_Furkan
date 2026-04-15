using MediatR;

namespace Coworking.APP.Features.Bookings
{
    public class BookingDeleteRequest : IRequest<BookingDeleteResponse>
    {
        public int Id { get; set; }
    }
}
