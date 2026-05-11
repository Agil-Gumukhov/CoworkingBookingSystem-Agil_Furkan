using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Rooms
{
    public class RoomCreateHandler : IRequestHandler<RoomCreateRequest, RoomCreateResponse>
    {
        private readonly RoomService _service;
        private readonly CoworkingDb _db;

        public RoomCreateHandler(RoomService service, CoworkingDb db)
        {
            _service = service;
            _db = db;
        }

        public async Task<RoomCreateResponse> Handle(RoomCreateRequest request, CancellationToken cancellationToken)
        {
            var branchExists = await _db.Branches.AnyAsync(b => b.Id == request.BranchId, cancellationToken);
            if (!branchExists)
                throw new Exception($"Branch with Id {request.BranchId} not found");

            var room = new Room
            {
                Name = request.Name,
                Capacity = request.Capacity,
                HasProjector = request.HasProjector,
                BranchId = request.BranchId
            };

            var success = await _service.CreateAsync(room, cancellationToken);

            if (!success)
                throw new Exception("Failed to create room");

            return new RoomCreateResponse
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                HasProjector = room.HasProjector,
                BranchId = room.BranchId,
                Message = "Room created successfully"
            };
        }
    }
}
