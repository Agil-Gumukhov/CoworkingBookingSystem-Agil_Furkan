namespace Coworking.APP.Features.Bookings
{
    public class BookingCreateResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public int? RoomId { get; set; }
        public int? DeskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
