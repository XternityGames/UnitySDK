using System;
using System.Diagnostics;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Xternity.Authentication;
using Xternity.REST;
using Xternity.REST.Responses;

namespace Xternity
{
    public class Auth
    {
        private static RESTAPI Api => RESTAPI.Shared;

        private static string _playerAuthToken = null;
        public static string PlayerAuthToken
        {
            get
            {
                if (_playerAuthToken == null)
                    throw new InvalidOperationException("Cannot fetch player auth token since no player is logged in by the system. Make sure you use the PlayerLogin API to log in a player first.");

                return _playerAuthToken;
            }
        }

        private Auth() { }

        /// <summary>
        /// Atttempts to login a player to the service with a custom proivder and auth token
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="authToken"></param>
        /// <returns></returns>
        private async static Task<PlayerLoginResponse> PlayerLogin(AuthProvider provider, string authToken)
        {
            var responseData = await Api.PlayerLogin<PlayerLoginResponse>(provider.ToString().ToLower(), authToken);

            if (responseData.Status.ToLower() == "error")
                UnityEngine.Debug.LogError(responseData.Error);
            else
                _playerAuthToken = responseData.Data.Tokens.Access.Token;

            return responseData;
        }

        public async static Task<PlayerLoginResponse> PlayerCustomLogin(string username, string password)
        {
            NetworkCredential creds = new NetworkCredential(username, password);

            Encoding ASCII = Encoding.ASCII;
            string basicEncrypt = creds.UserName + ":" + creds.Password;

            string token = Convert.ToBase64String(ASCII.GetBytes(basicEncrypt));
            return await PlayerLogin(AuthProvider.XG, token);
        }
    }

    internal enum AuthProvider
    {
        None = 0,
        Discord,
        XG, // cusotom login
        MetaMask,
        Phantom
    }
}