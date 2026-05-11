using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Desks
{
    public class DeskDeleteHandler : IRequestHandler<DeskDeleteRequest, DeskDeleteResponse>
    {
        private readonly DeskService _service;

        public DeskDeleteHandler(DeskService service)
        {
            _service = service;
        }

        public async Task<DeskDeleteResponse> Handle(DeskDeleteRequest request, CancellationToken cancellationToken)
        {
            var desk = await _service.GetDeskByIdAsync(request.Id, cancellationToken);

            if (desk == null)
                throw new Exception($"Desk with Id {request.Id} not found");

            await _service.DeleteDeskAsync(desk, cancellationToken);

            return new DeskDeleteResponse
            {
                Success = true,
                Message = "Desk deleted successfully"
            };
        }
    }
}
