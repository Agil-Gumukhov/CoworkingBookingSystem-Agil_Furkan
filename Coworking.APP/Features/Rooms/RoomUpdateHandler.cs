using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Rooms
{
    public class RoomUpdateHandler : IRequestHandler<RoomUpdateRequest, RoomUpdateResponse>
    {
        private readonly RoomService _service;
        private readonly CoworkingDb _db;

        public RoomUpdateHandler(RoomService service, CoworkingDb db)
        {
            _service = service;
            _db = db;
        }

        public async Task<RoomUpdateResponse> Handle(RoomUpdateRequest request, CancellationToken cancellationToken)
        {
            var room = await _service.GetByIdAsync(request.Id, cancellationToken);
            if (room == null)
                throw new Exception($"Room with Id {request.Id} not found");

            var branchExists = await _db.Branches.AnyAsync(b => b.Id == request.BranchId, cancellationToken);
            if (!branchExists)
                throw new Exception($"Branch with Id {request.BranchId} not found");

            room.Name = request.Name;
            room.Capacity = request.Capacity;
            room.HasProjector = request.HasProjector;
            room.BranchId = request.BranchId;

            var success = await _service.UpdateAsync(room, cancellationToken);

            if (!success)
                throw new Exception("Failed to update room");

            return new RoomUpdateResponse
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                HasProjector = room.HasProjector,
                BranchId = room.BranchId,
                Message = "Room updated successfully"
            };
        }
    }
}
