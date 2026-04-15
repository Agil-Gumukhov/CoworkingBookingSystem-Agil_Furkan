namespace Coworking.APP.Features.Desks
{
    public class DeskQueryResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Floor { get; set; }
        public bool IsPrivate { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
    }
}
