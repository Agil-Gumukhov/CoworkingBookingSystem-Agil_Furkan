using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Coworking.APP.Features.Desks
{
    public class DeskUpdateRequest : IRequest<DeskUpdateResponse>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Desk code is required")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Desk code must be between 1 and 20 characters")]
        public string Code { get; set; }

        [Range(0, 100, ErrorMessage = "Floor must be between 0 and 100")]
        public int Floor { get; set; }

        public bool IsPrivate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BranchId must be valid")]
        public int BranchId { get; set; }
    }
}
