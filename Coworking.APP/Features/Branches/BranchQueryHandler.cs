using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Branches
{
    public class BranchQueryHandler : IRequestHandler<BranchQueryRequest, BranchQueryResponse>
    {
        private readonly CoworkingDb _db;

        public BranchQueryHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<BranchQueryResponse> Handle(BranchQueryRequest request, CancellationToken cancellationToken)
        {
            var branch = await _db.Branches
                .Include(b => b.Rooms)
                .Include(b => b.Desks)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

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
        private readonly CoworkingDb _db;

        public BranchQueryAllHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<List<BranchQueryResponse>> Handle(BranchQueryAllRequest request, CancellationToken cancellationToken)
        {
            var branches = await _db.Branches
                .Include(b => b.Rooms)
                .Include(b => b.Desks)
                .Select(b => new BranchQueryResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    City = b.City,
                    RoomCount = b.Rooms.Count,
                    DeskCount = b.Desks.Count
                })
                .ToListAsync(cancellationToken);

            return branches;
        }
    }
}
