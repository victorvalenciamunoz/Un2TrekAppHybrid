namespace Un2TrekApp.Authentication;

internal class AuthenticationResponse
{
    public string JwtSecurityToken { get; set; }
    public DateTime ExpirationDate { get; set; }
}
