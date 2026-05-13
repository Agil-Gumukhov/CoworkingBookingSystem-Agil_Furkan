using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Amenities
{
    public class AmenityDeleteHandler : IRequestHandler<AmenityDeleteRequest, AmenityDeleteResponse>
    {
        private readonly AmenityService _service;

        public AmenityDeleteHandler(AmenityService service)
        {
            _service = service;
        }

        public async Task<AmenityDeleteResponse> Handle(AmenityDeleteRequest request, CancellationToken cancellationToken)
        {
            var amenity = await _service.GetAmenityByIdAsync(request.Id, cancellationToken);
            if (amenity == null)
                throw new Exception($"Amenity with Id {request.Id} not found");

            await _service.DeleteAmenityAsync(amenity, cancellationToken);

            return new AmenityDeleteResponse
            {
                Success = true,
                Message = "Amenity deleted successfully"
            };
        }
    }
}
