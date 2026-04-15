namespace Coworking.APP.Features.Branches
{
    public class BranchQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int RoomCount { get; set; }
        public int DeskCount { get; set; }
    }
}
