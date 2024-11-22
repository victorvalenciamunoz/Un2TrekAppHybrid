
using ErrorOr;
using Un2TrekApp.Domain;

namespace Un2TrekApp.Authentication;

internal interface IAuthService
{
    Task<ErrorOr<LoginResponse>> AuthenticateAsync(LoginRequest userCredentials);
    Task<User?> DoLoginAsync(LoginRequest userCredentials);
    Task<ErrorOr<Success>> RegisterAsync(RegistrationRequest registrationRequest);
}