using UnityEngine;
using System.Threading.Tasks;
using Xternity.REST.Responses;
using Xternity.REST;
using System.Runtime.CompilerServices;

namespace Xternity
{
    public class Core
    {
        private const string _apiUrl = "https://dev-app-api.xternity.games/";
        private static string _projectID;
        private static RESTAPI _restAPI;

        private Core() { }

        /// <summary>
        /// Initializes the service for use
        /// you have to call this before using the SDK
        /// </summary>
        public static void Init(string projectID)
        {
            _projectID = projectID;

            _restAPI = new RESTAPI(_apiUrl);
        }

        /// <summary>
        /// Gets the currently logged player's info
        /// </summary>
        public async static Task<PlayerInfoResponse> GetPlayerInfo()
        {
            return await _restAPI.GetPlayerInfo<PlayerInfoResponse>();
        }

        /// <summary>
        /// Gets the currently logged player's inventory
        /// </summary>
        public async static Task<PlayerInventoryResponse> GetPlayerInventory()
        {
            return await _restAPI.GetPlayerInventory<PlayerInventoryResponse>();
        }

        /// <summary>
        /// sends an asset to a user
        /// </summary>
        public async static Task<PlayerInventoryResponse> SendAssetToUser(string identifier, string value, string assetID, string tokenID)
        {
            return await _restAPI.SendAssetToUser<PlayerInventoryResponse>(identifier, value, assetID, tokenID);
        }
    }
}
