using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Amenities
{
    public class AmenityQueryHandler : IRequestHandler<AmenityQueryRequest, AmenityQueryResponse>
    {
        private readonly AmenityService _service;

        public AmenityQueryHandler(AmenityService service)
        {
            _service = service;
        }

        public async Task<AmenityQueryResponse> Handle(AmenityQueryRequest request, CancellationToken cancellationToken)
        {
            var amenity = await _service.GetAmenityByIdAsync(request.Id, cancellationToken);
            if (amenity == null)
                throw new Exception($"Amenity with Id {request.Id} not found");

            return new AmenityQueryResponse
            {
                Id = amenity.Id,
                Name = amenity.Name,
                Description = amenity.Description,
                IsPremium = amenity.IsPremium,
                BranchIds = amenity.BranchAmenities.Select(ba => ba.BranchId).ToList(),
                BranchNames = amenity.BranchAmenities.Select(ba => ba.Branch?.Name).Where(name => !string.IsNullOrWhiteSpace(name)).ToList()
            };
        }
    }

    public class AmenityQueryAllRequest : IRequest<List<AmenityQueryResponse>>
    {
    }

    public class AmenityQueryAllHandler : IRequestHandler<AmenityQueryAllRequest, List<AmenityQueryResponse>>
    {
        private readonly AmenityService _service;

        public AmenityQueryAllHandler(AmenityService service)
        {
            _service = service;
        }

        public async Task<List<AmenityQueryResponse>> Handle(AmenityQueryAllRequest request, CancellationToken cancellationToken)
        {
            var amenities = await _service.GetAllAmenitiesAsync(cancellationToken);
            return amenities.Select(amenity => new AmenityQueryResponse
            {
                Id = amenity.Id,
                Name = amenity.Name,
                Description = amenity.Description,
                IsPremium = amenity.IsPremium,
                BranchIds = amenity.BranchAmenities.Select(ba => ba.BranchId).ToList(),
                BranchNames = amenity.BranchAmenities.Select(ba => ba.Branch?.Name).Where(name => !string.IsNullOrWhiteSpace(name)).ToList()
            }).ToList();
        }
    }
}
