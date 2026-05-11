using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchQueryHandler : IRequestHandler<BranchQueryRequest, BranchQueryResponse>
    {
        private readonly BranchService _service;

        public BranchQueryHandler(BranchService service)
        {
            _service = service;
        }

        public async Task<BranchQueryResponse> Handle(BranchQueryRequest request, CancellationToken cancellationToken)
        {
            var branch = await _service.GetByIdAsync(request.Id, cancellationToken);

            if (branch == null)
                throw new Exception($"Branch with Id {request.Id} not found");

            return new BranchQueryResponse
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                City = branch.City,
                RoomCount = branch.Rooms.Count,
                DeskCount = branch.Desks.Count
            };
        }
    }

    public class BranchQueryAllRequest : IRequest<List<BranchQueryResponse>>
    {
    }

    public class BranchQueryAllHandler : IRequestHandler<BranchQueryAllRequest, List<BranchQueryResponse>>
    {
        private readonly BranchService _service;

        public BranchQueryAllHandler(BranchService service)
        {
            _service = service;
        }

        public async Task<List<BranchQueryResponse>> Handle(BranchQueryAllRequest request, CancellationToken cancellationToken)
        {
            var branches = await _service.GetAllAsync(cancellationToken);

            return branches.Select(b => new BranchQueryResponse
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                RoomCount = b.Rooms.Count,
                DeskCount = b.Desks.Count
            }).ToList();
        }
    }
}
