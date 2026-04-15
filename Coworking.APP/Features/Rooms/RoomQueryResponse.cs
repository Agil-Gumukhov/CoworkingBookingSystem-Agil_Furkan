namespace Coworking.APP.Features.Rooms
{
    public class RoomQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool HasProjector { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
    }
}
