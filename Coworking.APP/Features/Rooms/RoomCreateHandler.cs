using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Rooms
{
    public class RoomCreateHandler : IRequestHandler<RoomCreateRequest, RoomCreateResponse>
    {
        private readonly RoomService _service;

        public RoomCreateHandler(RoomService service)
        {
            _service = service;
        }

        public async Task<RoomCreateResponse> Handle(RoomCreateRequest request, CancellationToken cancellationToken)
        {
            var room = new Room
            {
                Name = request.Name,
                Capacity = request.Capacity,
                HourlyRate = request.HourlyRate,
                HasProjector = request.HasProjector,
                BranchId = request.BranchId
            };

            room = await _service.CreateRoomAsync(room, cancellationToken);

            return new RoomCreateResponse
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                HourlyRate = room.HourlyRate,
                HasProjector = room.HasProjector,
                BranchId = room.BranchId,
                Message = "Room created successfully"
            };
        }
    }
}
