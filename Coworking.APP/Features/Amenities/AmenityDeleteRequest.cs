using MediatR;

namespace Coworking.APP.Features.Amenities
{
    public class AmenityDeleteRequest : IRequest<AmenityDeleteResponse>
    {
        public int Id { get; set; }
    }
}
