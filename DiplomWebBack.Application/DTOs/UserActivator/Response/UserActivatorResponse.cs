namespace DiplomWebBack.Application.DTOs.UserActivator.Response
{
    public class UserActivatorResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserPatronymic { get; set; }
        public string UserEmail { get; set; }
    }
}
