using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Desks
{
    public class DeskCreateHandler : IRequestHandler<DeskCreateRequest, DeskCreateResponse>
    {
        private readonly CoworkingDb _db;

        public DeskCreateHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<DeskCreateResponse> Handle(DeskCreateRequest request, CancellationToken cancellationToken)
        {
            var branchExists = await _db.Branches.AnyAsync(b => b.Id == request.BranchId, cancellationToken);
            if (!branchExists)
                throw new Exception($"Branch with Id {request.BranchId} not found");

            var desk = new Desk
            {
                Code = request.Code,
                Floor = request.Floor,
                IsPrivate = request.IsPrivate,
                BranchId = request.BranchId
            };

            _db.Desks.Add(desk);
            await _db.SaveChangesAsync(cancellationToken);

            return new DeskCreateResponse
            {
                Id = desk.Id,
                Code = desk.Code,
                Floor = desk.Floor,
                IsPrivate = desk.IsPrivate,
                BranchId = desk.BranchId,
                Message = "Desk created successfully"
            };
        }
    }
}
