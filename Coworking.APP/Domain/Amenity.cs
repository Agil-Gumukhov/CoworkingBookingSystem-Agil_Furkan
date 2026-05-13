using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Coworking.APP.Domain
{
    public class Amenity : Entity
    {
        [Required(ErrorMessage = "Amenity name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Amenity name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string Description { get; set; }

        public bool IsPremium { get; set; }

        public List<BranchAmenity> BranchAmenities { get; set; } = new List<BranchAmenity>();
    }
}
