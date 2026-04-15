using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Coworking.APP.Features.Bookings
{
    public class BookingUpdateRequest : IRequest<BookingUpdateResponse>
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "UserId must be valid")]
        public int UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BranchId must be valid")]
        public int BranchId { get; set; }

        public int? RoomId { get; set; }

        public int? DeskId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50, MinimumLength = 1)]
        public string Status { get; set; }
    }
}
