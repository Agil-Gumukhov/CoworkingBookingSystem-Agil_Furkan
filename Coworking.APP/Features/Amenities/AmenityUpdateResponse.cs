namespace Coworking.APP.Features.Amenities
{
    public class AmenityUpdateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPremium { get; set; }
        public List<int> BranchIds { get; set; } = new List<int>();
        public string Message { get; set; }
    }
}
