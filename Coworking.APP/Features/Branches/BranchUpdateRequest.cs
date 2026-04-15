using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchUpdateRequest : IRequest<BranchUpdateResponse>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Branch name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 255 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "City must be between 2 and 50 characters")]
        public string City { get; set; }
    }
}
