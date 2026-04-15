using MediatR;

namespace Coworking.APP.Features.Desks
{
    public class DeskDeleteRequest : IRequest<DeskDeleteResponse>
    {
        public int Id { get; set; }
    }
}
