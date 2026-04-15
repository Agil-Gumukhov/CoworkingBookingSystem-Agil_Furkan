using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Desks
{
    public class DeskUpdateHandler : IRequestHandler<DeskUpdateRequest, DeskUpdateResponse>
    {
        private readonly CoworkingDb _db;

        public DeskUpdateHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<DeskUpdateResponse> Handle(DeskUpdateRequest request, CancellationToken cancellationToken)
        {
            var desk = await _db.Desks.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (desk == null)
                throw new Exception($"Desk with Id {request.Id} not found");

            var branchExists = await _db.Branches.AnyAsync(b => b.Id == request.BranchId, cancellationToken);
            if (!branchExists)
                throw new Exception($"Branch with Id {request.BranchId} not found");

            desk.Code = request.Code;
            desk.Floor = request.Floor;
            desk.IsPrivate = request.IsPrivate;
            desk.BranchId = request.BranchId;

            _db.Desks.Update(desk);
            await _db.SaveChangesAsync(cancellationToken);

            return new DeskUpdateResponse
            {
                Id = desk.Id,
                Code = desk.Code,
                Floor = desk.Floor,
                IsPrivate = desk.IsPrivate,
                BranchId = desk.BranchId,
                Message = "Desk updated successfully"
            };
        }
    }
}
