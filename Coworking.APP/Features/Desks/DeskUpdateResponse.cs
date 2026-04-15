namespace Coworking.APP.Features.Desks
{
    public class DeskUpdateResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Floor { get; set; }
        public bool IsPrivate { get; set; }
        public int BranchId { get; set; }
        public string Message { get; set; }
    }
}
