using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Rooms
{
    public class RoomQueryHandler : IRequestHandler<RoomQueryRequest, RoomQueryResponse>
    {
        private readonly CoworkingDb _db;

        public RoomQueryHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<RoomQueryResponse> Handle(RoomQueryRequest request, CancellationToken cancellationToken)
        {
            var room = await _db.Rooms
                .Include(r => r.Branch)
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (room == null)
                throw new Exception($"Room with Id {request.Id} not found");

            return new RoomQueryResponse
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                HasProjector = room.HasProjector,
                BranchId = room.BranchId,
                BranchName = room.Branch?.Name
            };
        }
    }

    public class RoomQueryAllRequest : IRequest<List<RoomQueryResponse>>
    {
    }

    public class RoomQueryAllHandler : IRequestHandler<RoomQueryAllRequest, List<RoomQueryResponse>>
    {
        private readonly CoworkingDb _db;

        public RoomQueryAllHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<List<RoomQueryResponse>> Handle(RoomQueryAllRequest request, CancellationToken cancellationToken)
        {
            var rooms = await _db.Rooms
                .Include(r => r.Branch)
                .Select(r => new RoomQueryResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    Capacity = r.Capacity,
                    HasProjector = r.HasProjector,
                    BranchId = r.BranchId,
                    BranchName = r.Branch.Name
                })
                .ToListAsync(cancellationToken);

            return rooms;
        }
    }
}
