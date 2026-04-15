using Coworking.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Features.Desks
{
    public class DeskDeleteHandler : IRequestHandler<DeskDeleteRequest, DeskDeleteResponse>
    {
        private readonly CoworkingDb _db;

        public DeskDeleteHandler(CoworkingDb db)
        {
            _db = db;
        }

        public async Task<DeskDeleteResponse> Handle(DeskDeleteRequest request, CancellationToken cancellationToken)
        {
            var desk = await _db.Desks
                .Include(d => d.Bookings)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (desk == null)
                throw new Exception($"Desk with Id {request.Id} not found");

            if (desk.Bookings.Any())
                throw new Exception("Cannot delete desk that has associated bookings");

            _db.Desks.Remove(desk);
            await _db.SaveChangesAsync(cancellationToken);

            return new DeskDeleteResponse
            {
                Success = true,
                Message = "Desk deleted successfully"
            };
        }
    }
}
