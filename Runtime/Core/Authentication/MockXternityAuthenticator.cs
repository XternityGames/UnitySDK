using System;
using System.Net;
using System.Text;

namespace Xternity.Samples
{
    public class MockXternityAuthenticator : Xternity.Authentication.IAuthenticationProvider
    {
        public string Name => "xg";

        private string _authToken;
        public string AuthToken => "ZG9yQHh0ZXJuaXR5LmdhbWVzOnhnMTIzNA==";
    }
}

// see https://learn.microsoft.com/en-us/dotnet/api/system.net.iauthenticationmodule?view=net-7.0
public class CustomLogin : IAuthenticationModule
{
    public string AuthenticationType { get; } = "xg";
    public bool CanPreAuthenticate { get; }

    // The CheckChallenge method checks whether the challenge sent by the HttpWebRequest
    // contains the correct type (Basic) and the correct domain name.
    // Note: The challenge is in the form BASIC REALM="DOMAINNAME";
    // the Internet Web site must reside on a server whose
    // domain name is equal to DOMAINNAME.
    public bool CheckChallenge(string challenge, string domain)
    {
        bool challengePasses = false;

        String tempChallenge = challenge.ToUpper();

        // Verify that this is a Basic authorization request and that the requested domain
        // is correct.
        // Note: When the domain is an empty string, the following code only checks
        // whether the authorization type is Basic.

        if (tempChallenge.IndexOf("BASIC") != -1)
            if (!string.IsNullOrEmpty(domain))
                if (tempChallenge.IndexOf(domain.ToUpper()) != -1)
                    challengePasses = true;
                else
                    // The domain is not allowed and the authorization type is Basic.
                    challengePasses = false;
            else
                // The domain is a blank string and the authorization type is Basic.
                challengePasses = true;

        return challengePasses;
    }
    public Authorization PreAuthenticate(WebRequest request, ICredentials credentials)
    {
        return null;
    }

    // Authenticate is the core method for this custom authentication.
    // When an Internet resource requests authentication, the WebRequest.GetResponse
    // method calls the AuthenticationManager.Authenticate method. This method, in
    // turn, calls the Authenticate method on each of the registered authentication
    // modules, in the order in which they were registered. When the authentication is
    // complete an Authorization object is returned to the WebRequest.
    public Authorization Authenticate(String challenge, WebRequest request, ICredentials credentials)
    {
        Encoding ASCII = Encoding.ASCII;

        // Get the username and password from the credentials
        NetworkCredential myCreds = credentials.GetCredential(request.RequestUri, AuthenticationType);
        // Create the encrypted string according to the Basic authentication format as
        // follows:
        // a)Concatenate the username and password separated by colon;
        // b)Apply ASCII encoding to obtain a stream of bytes;
        // c)Apply Base64 encoding to this array of bytes to obtain the encoded
        // authorization.
        string basicEncrypt = myCreds.UserName + ":" + myCreds.Password;

        string basicToken = AuthenticationType + " " + Convert.ToBase64String(ASCII.GetBytes(basicEncrypt));

        // Create an Authorization object using the encoded authorization above.
        Authorization resourceAuthorization = new Authorization(basicToken);

        // Get the Message property, which contains the authorization string that the
        // client returns to the server when accessing protected resources.
        UnityEngine.Debug.Log(string.Format("\n Authorization Message:{0}", resourceAuthorization.Message));

        // Get the Complete property, which is set to true when the authentication process
        // between the client and the server is finished.
        UnityEngine.Debug.Log(string.Format("\n Authorization Complete:{0}", resourceAuthorization.Complete));

        UnityEngine.Debug.Log(string.Format("\n Authorization ConnectionGroupId:{0}", resourceAuthorization.ConnectionGroupId));

        return resourceAuthorization;
    }
}