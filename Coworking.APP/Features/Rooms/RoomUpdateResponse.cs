namespace Coworking.APP.Features.Rooms
{
    public class RoomUpdateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool HasProjector { get; set; }
        public int BranchId { get; set; }
        public string Message { get; set; }
    }
}
