using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Desks
{
    public class DeskUpdateHandler : IRequestHandler<DeskUpdateRequest, DeskUpdateResponse>
    {
        private readonly DeskService _service;

        public DeskUpdateHandler(DeskService service)
        {
            _service = service;
        }

        public async Task<DeskUpdateResponse> Handle(DeskUpdateRequest request, CancellationToken cancellationToken)
        {
            var desk = await _service.GetDeskByIdAsync(request.Id, cancellationToken);
            if (desk == null)
                throw new Exception($"Desk with Id {request.Id} not found");

            desk.Code = request.Code;
            desk.Floor = request.Floor;
            desk.HourlyRate = request.HourlyRate;
            desk.IsPrivate = request.IsPrivate;
            desk.BranchId = request.BranchId;

            desk = await _service.UpdateDeskAsync(desk, cancellationToken);

            return new DeskUpdateResponse
            {
                Id = desk.Id,
                Code = desk.Code,
                Floor = desk.Floor,
                HourlyRate = desk.HourlyRate,
                IsPrivate = desk.IsPrivate,
                BranchId = desk.BranchId,
                Message = "Desk updated successfully"
            };
        }
    }
}
