using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Desks
{
    public class DeskCreateHandler : IRequestHandler<DeskCreateRequest, DeskCreateResponse>
    {
        private readonly DeskService _service;

        public DeskCreateHandler(DeskService service)
        {
            _service = service;
        }

        public async Task<DeskCreateResponse> Handle(DeskCreateRequest request, CancellationToken cancellationToken)
        {
            var desk = new Desk
            {
                Code = request.Code,
                Floor = request.Floor,
                HourlyRate = request.HourlyRate,
                IsPrivate = request.IsPrivate,
                BranchId = request.BranchId
            };

            desk = await _service.CreateDeskAsync(desk, cancellationToken);

            return new DeskCreateResponse
            {
                Id = desk.Id,
                Code = desk.Code,
                Floor = desk.Floor,
                HourlyRate = desk.HourlyRate,
                IsPrivate = desk.IsPrivate,
                BranchId = desk.BranchId,
                Message = "Desk created successfully"
            };
        }
    }
}
