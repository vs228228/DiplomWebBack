namespace DiplomWebBack.Domain.Entities.Responses
{
    public class Notification
    {
        public Notification()
        {

        }

        public Notification(string title, string message, int code)
        {
            Title = title;
            Message = message;
            Code = code;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public int Code { get; set; } = default!;
    }
}
