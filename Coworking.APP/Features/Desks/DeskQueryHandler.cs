using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Desks
{
    public class DeskQueryHandler : IRequestHandler<DeskQueryRequest, DeskQueryResponse>
    {
        private readonly DeskService _service;

        public DeskQueryHandler(DeskService service)
        {
            _service = service;
        }

        public async Task<DeskQueryResponse> Handle(DeskQueryRequest request, CancellationToken cancellationToken)
        {
            var desk = await _service.GetDeskByIdAsync(request.Id, cancellationToken);

            if (desk == null)
                throw new Exception($"Desk with Id {request.Id} not found");

            return new DeskQueryResponse
            {
                Id = desk.Id,
                Code = desk.Code,
                Floor = desk.Floor,
                HourlyRate = desk.HourlyRate,
                IsPrivate = desk.IsPrivate,
                BranchId = desk.BranchId,
                BranchName = desk.Branch?.Name
            };
        }
    }

    public class DeskQueryAllRequest : IRequest<List<DeskQueryResponse>>
    {
    }

    public class DeskQueryAllHandler : IRequestHandler<DeskQueryAllRequest, List<DeskQueryResponse>>
    {
        private readonly DeskService _service;

        public DeskQueryAllHandler(DeskService service)
        {
            _service = service;
        }

        public async Task<List<DeskQueryResponse>> Handle(DeskQueryAllRequest request, CancellationToken cancellationToken)
        {
            var desks = await _service.GetAllDesksAsync(cancellationToken);
            return desks.Select(d => new DeskQueryResponse
            {
                Id = d.Id,
                Code = d.Code,
                Floor = d.Floor,
                HourlyRate = d.HourlyRate,
                IsPrivate = d.IsPrivate,
                BranchId = d.BranchId,
                BranchName = d.Branch?.Name
            }).ToList();
        }
    }
}
