using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Desks
{
    public class DeskQueryHandler : IRequestHandler<DeskQueryRequest, DeskQueryResponse>
    {
        private readonly CoworkingDb _db;

        public DeskQueryHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<DeskQueryResponse> Handle(DeskQueryRequest request, CancellationToken cancellationToken)
        {
            var desk = await _db.Desks
                .Include(d => d.Branch)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (desk == null)
                throw new Exception($"Desk with Id {request.Id} not found");

            return new DeskQueryResponse
            {
                Id = desk.Id,
                Code = desk.Code,
                Floor = desk.Floor,
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
        private readonly CoworkingDb _db;

        public DeskQueryAllHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<List<DeskQueryResponse>> Handle(DeskQueryAllRequest request, CancellationToken cancellationToken)
        {
            var desks = await _db.Desks
                .Include(d => d.Branch)
                .Select(d => new DeskQueryResponse
                {
                    Id = d.Id,
                    Code = d.Code,
                    Floor = d.Floor,
                    IsPrivate = d.IsPrivate,
                    BranchId = d.BranchId,
                    BranchName = d.Branch.Name
                })
                .ToListAsync(cancellationToken);

            return desks;
        }
    }
}
