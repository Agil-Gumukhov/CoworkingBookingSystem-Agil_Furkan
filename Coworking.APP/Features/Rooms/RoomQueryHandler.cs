using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Rooms
{
    public class RoomQueryHandler : IRequestHandler<RoomQueryRequest, RoomQueryResponse>
    {
        private readonly RoomService _service;

        public RoomQueryHandler(RoomService service)
        {
            _service = service;
        }

        public async Task<RoomQueryResponse> Handle(RoomQueryRequest request, CancellationToken cancellationToken)
        {
            var room = await _service.GetRoomByIdAsync(request.Id, cancellationToken);

            if (room == null)
                throw new Exception($"Room with Id {request.Id} not found");

            return new RoomQueryResponse
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                HourlyRate = room.HourlyRate,
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
        private readonly RoomService _service;

        public RoomQueryAllHandler(RoomService service)
        {
            _service = service;
        }

        public async Task<List<RoomQueryResponse>> Handle(RoomQueryAllRequest request, CancellationToken cancellationToken)
        {
            var rooms = await _service.GetAllRoomsAsync(cancellationToken);
            return rooms.Select(r => new RoomQueryResponse
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                HourlyRate = r.HourlyRate,
                HasProjector = r.HasProjector,
                BranchId = r.BranchId,
                BranchName = r.Branch?.Name
            }).ToList();
        }
    }
}
