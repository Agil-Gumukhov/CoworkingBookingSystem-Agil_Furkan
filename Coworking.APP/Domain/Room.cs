using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Coworking.APP.Domain
{
    public class Room : Entity
    {
        [Required(ErrorMessage = "Room name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Room name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [Range(1, 100, ErrorMessage = "Capacity must be between 1 and 100")]
        public int Capacity { get; set; }

        public bool HasProjector { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BranchId must be valid")]
        public int BranchId { get; set; }

        public Branch Branch { get; set; }

        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
