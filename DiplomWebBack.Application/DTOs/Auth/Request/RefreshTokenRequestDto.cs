namespace DiplomWebBack.Application.DTOs.Auth.Request
{
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
