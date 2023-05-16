namespace Xternity.Authentication
{
    public interface IAuthenticationProvider
    {
        string Name { get; }
        string AuthToken { get; }
    }
}