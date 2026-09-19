namespace EnhanzerAssessment.Api.DTOs
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ExternalLoginBody
    {
        public string Username { get; set; } = string.Empty;
        public string Pw { get; set; } = string.Empty;
    }

    public class ExternalLoginRequest
    {
        public string API_Action { get; set; } = "GetLoginData";
        public string Device_Id { get; set; } = "D001";
        public string Sync_Time { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public ExternalLoginBody API_Body { get; set; } = new ExternalLoginBody();
    }
}
