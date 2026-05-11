using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Rooms
{
    public class RoomDeleteHandler : IRequestHandler<RoomDeleteRequest, RoomDeleteResponse>
    {
        private readonly RoomService _service;

        public RoomDeleteHandler(RoomService service)
        {
            _service = service;
        }

        public async Task<RoomDeleteResponse> Handle(RoomDeleteRequest request, CancellationToken cancellationToken)
        {
            var room = await _service.GetRoomByIdAsync(request.Id, cancellationToken);

            if (room == null)
                throw new Exception($"Room with Id {request.Id} not found");

            await _service.DeleteRoomAsync(room, cancellationToken);

            return new RoomDeleteResponse
            {
                Success = true,
                Message = "Room deleted successfully"
            };
        }
    }
}
