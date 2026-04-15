using MediatR;

namespace Coworking.APP.Features.Bookings
{
    public class BookingQueryRequest : IRequest<BookingQueryResponse>
    {
        public int Id { get; set; }
    }
}
