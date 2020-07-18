namespace PE.Contracts.V1.Responses
{
    public class AuthSuccessResponse
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
