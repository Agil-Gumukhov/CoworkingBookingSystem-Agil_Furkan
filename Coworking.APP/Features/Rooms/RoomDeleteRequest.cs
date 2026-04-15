using MediatR;

namespace Coworking.APP.Features.Rooms
{
    public class RoomDeleteRequest : IRequest<RoomDeleteResponse>
    {
        public int Id { get; set; }
    }
}
