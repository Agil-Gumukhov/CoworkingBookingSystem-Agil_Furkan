using MediatR;

namespace Coworking.APP.Features.Amenities
{
    public class AmenityQueryRequest : IRequest<AmenityQueryResponse>
    {
        public int Id { get; set; }
    }
}
