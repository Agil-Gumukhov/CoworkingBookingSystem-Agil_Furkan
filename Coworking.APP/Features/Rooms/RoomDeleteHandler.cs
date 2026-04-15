using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Rooms
{
    public class RoomDeleteHandler : IRequestHandler<RoomDeleteRequest, RoomDeleteResponse>
    {
        private readonly CoworkingDb _db;

        public RoomDeleteHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<RoomDeleteResponse> Handle(RoomDeleteRequest request, CancellationToken cancellationToken)
        {
            var room = await _db.Rooms
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (room == null)
                throw new Exception($"Room with Id {request.Id} not found");

            if (room.Bookings.Any())
                throw new Exception("Cannot delete room that has associated bookings");

            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync(cancellationToken);

            return new RoomDeleteResponse
            {
                Success = true,
                Message = "Room deleted successfully"
            };
        }
    }
}
