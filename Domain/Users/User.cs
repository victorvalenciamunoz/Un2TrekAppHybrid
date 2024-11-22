namespace Un2TrekApp.Domain;

internal class User
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Pass { get; set; } = string.Empty;
    public string UserRoles { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    public AuthenticationData? AuthData { get; set; }
}
internal class AuthenticationData
{
    public string JwtSecurityToken { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
}
