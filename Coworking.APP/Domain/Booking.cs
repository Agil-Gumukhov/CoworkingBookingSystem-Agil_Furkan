using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Coworking.APP.Domain
{
    public class Booking : Entity
    {
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be valid")]
        public int UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BranchId must be valid")]
        public int BranchId { get; set; }

        public Branch Branch { get; set; }

        public int? RoomId { get; set; }
        public Room Room { get; set; }

        public int? DeskId { get; set; }
        public Desk Desk { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Status must be between 1 and 50 characters")]
        public string Status { get; set; }
    }
}