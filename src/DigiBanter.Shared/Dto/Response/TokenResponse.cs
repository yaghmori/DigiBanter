namespace DigiBanter.Shared.Dto.Response
{
    public class TokenResponse
    {


        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public long AccessTokenExpires { get; set; }
        public long RefreshTokenExpires { get; set; }
        public string Scheme { get; set; }

    }
}
