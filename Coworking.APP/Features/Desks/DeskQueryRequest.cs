using MediatR;

namespace Coworking.APP.Features.Desks
{
    public class DeskQueryRequest : IRequest<DeskQueryResponse>
    {
        public int Id { get; set; }
    }
}
