using MediatR;

namespace Coworking.APP.Features.Rooms
{
    public class RoomQueryRequest : IRequest<RoomQueryResponse>
    {
        public int Id { get; set; }
    }
}
