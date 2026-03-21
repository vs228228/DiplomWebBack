namespace DiplomWebBack.Application.DTOs.User.Request
{
    public class UserPaginatedRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
      //  public string SearchBy { get; set; }
    }
}
