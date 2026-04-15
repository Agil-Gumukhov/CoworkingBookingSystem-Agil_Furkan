using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Rooms
{
    public class RoomCreateHandler : IRequestHandler<RoomCreateRequest, RoomCreateResponse>
    {
        private readonly CoworkingDb _db;

        public RoomCreateHandler(CoworkingDb db)
        {
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

            _db.Rooms.Add(room);
            await _db.SaveChangesAsync(cancellationToken);

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
