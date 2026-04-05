namespace DiplomWebBack.Domain.Entities.Responses
{
    public class Notification
    {
        public Notification()
        {

        }

        public Notification(string title, string message, string code)
        {
            Title = title;
            Message = message;
            Code = code;
        }

        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
