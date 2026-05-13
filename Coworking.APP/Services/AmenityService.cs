using CORE.APP.Services;
using Coworking.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace Coworking.APP.Services
{
    public class AmenityService : Service<Amenity>
    {
        public AmenityService(CoworkingDb db) : base(db)
        {
        }

        public async Task<List<Amenity>> GetAllAmenitiesAsync(CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(a => a.BranchAmenities)
                .ThenInclude(ba => ba.Branch)
                .ToListAsync(cancellationToken);
        }

        public async Task<Amenity> GetAmenityByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet()
                .Include(a => a.BranchAmenities)
                .ThenInclude(ba => ba.Branch)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Amenity> CreateAmenityAsync(Amenity amenity, CancellationToken cancellationToken)
        {
            await ValidateBranchIdsAsync(amenity.BranchAmenities.Select(ba => ba.BranchId).ToList(), cancellationToken);
            await CreateAsync(amenity, cancellationToken);
            return amenity;
        }

        public async Task<Amenity> UpdateAmenityAsync(Amenity amenity, CancellationToken cancellationToken)
        {
            await ValidateBranchIdsAsync(amenity.BranchAmenities.Select(ba => ba.BranchId).ToList(), cancellationToken);
            await UpdateAsync(amenity, cancellationToken);
            return amenity;
        }

        public async Task DeleteAmenityAsync(Amenity amenity, CancellationToken cancellationToken)
        {
            var amenityWithRelations = await DbSet()
                .Include(a => a.BranchAmenities)
                .FirstOrDefaultAsync(a => a.Id == amenity.Id, cancellationToken);

            if (amenityWithRelations?.BranchAmenities.Any() == true)
                Delete(amenityWithRelations.BranchAmenities);

            await DeleteAsync(amenity, cancellationToken);
        }

        private async Task ValidateBranchIdsAsync(List<int> branchIds, CancellationToken cancellationToken)
        {
            if (!branchIds.Any())
                return;

            var existingBranchIds = await DbSet<Branch>()
                .Where(b => branchIds.Contains(b.Id))
                .Select(b => b.Id)
                .ToListAsync(cancellationToken);

            var missingBranchIds = branchIds.Except(existingBranchIds).ToList();
            if (missingBranchIds.Any())
                throw new InvalidOperationException($"Branch ids not found: {string.Join(", ", missingBranchIds)}");
        }
    }
}
