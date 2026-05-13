using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Amenities
{
    public class AmenityUpdateHandler : IRequestHandler<AmenityUpdateRequest, AmenityUpdateResponse>
    {
        private readonly AmenityService _service;

        public AmenityUpdateHandler(AmenityService service)
        {
            _service = service;
        }

        public async Task<AmenityUpdateResponse> Handle(AmenityUpdateRequest request, CancellationToken cancellationToken)
        {
            var amenity = await _service.GetAmenityByIdAsync(request.Id, cancellationToken);
            if (amenity == null)
                throw new Exception($"Amenity with Id {request.Id} not found");

            amenity.Name = request.Name;
            amenity.Description = request.Description;
            amenity.IsPremium = request.IsPremium;
            amenity.BranchAmenities = request.BranchIds.Distinct().Select(branchId => new BranchAmenity
            {
                BranchId = branchId,
                AmenityId = amenity.Id
            }).ToList();

            amenity = await _service.UpdateAmenityAsync(amenity, cancellationToken);

            return new AmenityUpdateResponse
            {
                Id = amenity.Id,
                Name = amenity.Name,
                Description = amenity.Description,
                IsPremium = amenity.IsPremium,
                BranchIds = amenity.BranchAmenities.Select(ba => ba.BranchId).ToList(),
                Message = "Amenity updated successfully"
            };
        }
    }
}
