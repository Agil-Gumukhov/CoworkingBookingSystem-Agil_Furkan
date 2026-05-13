using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Amenities
{
    public class AmenityCreateHandler : IRequestHandler<AmenityCreateRequest, AmenityCreateResponse>
    {
        private readonly AmenityService _service;

        public AmenityCreateHandler(AmenityService service)
        {
            _service = service;
        }

        public async Task<AmenityCreateResponse> Handle(AmenityCreateRequest request, CancellationToken cancellationToken)
        {
            var amenity = new Amenity
            {
                Name = request.Name,
                Description = request.Description,
                IsPremium = request.IsPremium,
                BranchAmenities = request.BranchIds.Distinct().Select(branchId => new BranchAmenity
                {
                    BranchId = branchId
                }).ToList()
            };

            amenity = await _service.CreateAmenityAsync(amenity, cancellationToken);

            return new AmenityCreateResponse
            {
                Id = amenity.Id,
                Name = amenity.Name,
                Description = amenity.Description,
                IsPremium = amenity.IsPremium,
                BranchIds = amenity.BranchAmenities.Select(ba => ba.BranchId).ToList(),
                Message = "Amenity created successfully"
            };
        }
    }
}
