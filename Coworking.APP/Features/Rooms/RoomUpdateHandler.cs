using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Rooms
{
    public class RoomUpdateHandler : IRequestHandler<RoomUpdateRequest, RoomUpdateResponse>
    {
        private readonly RoomService _service;

        public RoomUpdateHandler(RoomService service)
        {
            _service = service;
        }

        public async Task<RoomUpdateResponse> Handle(RoomUpdateRequest request, CancellationToken cancellationToken)
        {
            var room = await _service.GetRoomByIdAsync(request.Id, cancellationToken);
            if (room == null)
                throw new Exception($"Room with Id {request.Id} not found");

            room.Name = request.Name;
            room.Capacity = request.Capacity;
            room.HasProjector = request.HasProjector;
            room.BranchId = request.BranchId;

            room = await _service.UpdateRoomAsync(room, cancellationToken);

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
