using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Rooms
{
    public class RoomUpdateHandler : IRequestHandler<RoomUpdateRequest, RoomUpdateResponse>
    {
        private readonly CoworkingDb _db;

        public RoomUpdateHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<RoomUpdateResponse> Handle(RoomUpdateRequest request, CancellationToken cancellationToken)
        {
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (room == null)
                throw new Exception($"Room with Id {request.Id} not found");

            var branchExists = await _db.Branches.AnyAsync(b => b.Id == request.BranchId, cancellationToken);
            if (!branchExists)
                throw new Exception($"Branch with Id {request.BranchId} not found");

            room.Name = request.Name;
            room.Capacity = request.Capacity;
            room.HasProjector = request.HasProjector;
            room.BranchId = request.BranchId;

            _db.Rooms.Update(room);
            await _db.SaveChangesAsync(cancellationToken);

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
