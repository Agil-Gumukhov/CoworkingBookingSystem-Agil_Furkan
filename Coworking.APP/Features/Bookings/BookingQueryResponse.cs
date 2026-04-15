namespace Coworking.APP.Features.Bookings
{
    public class BookingQueryResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public int? DeskId { get; set; }
        public string DeskCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
