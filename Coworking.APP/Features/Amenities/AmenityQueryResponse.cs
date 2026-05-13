namespace Coworking.APP.Features.Amenities
{
    public class AmenityQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPremium { get; set; }
        public List<int> BranchIds { get; set; } = new List<int>();
        public List<string> BranchNames { get; set; } = new List<string>();
    }
}
