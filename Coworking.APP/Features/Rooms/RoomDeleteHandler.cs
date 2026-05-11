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
            var room = await _service.GetByIdAsync(request.Id, cancellationToken);

            if (room == null)
                throw new Exception($"Room with Id {request.Id} not found");

            var hasRelatedData = await _service.HasRelatedDataAsync(request.Id, cancellationToken);
            if (hasRelatedData)
                throw new Exception("Cannot delete room that has associated bookings");

            var success = await _service.DeleteAsync(room, cancellationToken);

            if (!success)
                throw new Exception("Failed to delete room");

            return new RoomDeleteResponse
            {
                Success = true,
                Message = "Room deleted successfully"
            };
        }
    }
}
