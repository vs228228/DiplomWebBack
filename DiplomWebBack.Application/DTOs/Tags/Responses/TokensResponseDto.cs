namespace DiplomWebBack.Application.DTOs.Tags.Responses
{
    public class TokensResponseDto
    {
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpires { get; set; }
        public string AccessToken { get; set; }
        // возможно стоит добавить и для access токена
    }
}