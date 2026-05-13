using CORE.APP.Domain;

namespace Coworking.APP.Domain
{
    public class BranchAmenity : Entity
    {
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int AmenityId { get; set; }
        public Amenity Amenity { get; set; }
    }
}
