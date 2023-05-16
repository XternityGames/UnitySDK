using System.Collections.Generic;
using System.Net;
using System.Security;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Xternity.Authentication;
using Xternity.REST.Responses;
using static Xternity.REST.Responses.PlayerInventoryResponse;

namespace Xternity.Samples
{
    public class SDKTester : MonoBehaviour
    {
        [Header("Parameters")]
        public string ProjectID;

        [Header("General Refs")]
        public GameObject AuthenticationView;
        public GameObject PlayerActions;
        public GameObject Buffering;
        public TextMeshProUGUI ConsoleText;

        [Header("Player Info")]
        public GameObject PlayerInfoView;
        public TMP_Text PlayerName;
        public TMP_Text Email;
        public TMP_Text Wallet;

        [Header("Player Inventory")]
        public GameObject PlayerInventoryView;
        public NFTView NFTView_Prefab;

        [Header("Credentials")]
        public GameObject CredentialsView;
        public TMP_InputField UserName;
        public TMP_InputField Password;
        public Button Back;
        public Button Confirm;

        [Header("SendAsset")]
        public GameObject SendAssetView;
        public TMP_InputField FriendEmail;
        public TMP_Text AssetName;
        public Image AssetView;
        public Button Back_SA;
        public Button Confirm_SA;

        private List<GameObject> NFTViews = new List<GameObject>(6);
        private PlayerInventoryResponse_Result _currentAsset;

        private void Start()
        {
            Xternity.Core.Init(ProjectID);

            AuthenticationView.SetActive(true);
            PlayerActions.SetActive(false);
            CredentialsView.SetActive(false);
            SendAssetView.SetActive(false);

            Confirm.onClick.AddListener(Login);
            Back.onClick.AddListener(HideCredentials);

            Back_SA.onClick.AddListener(CloseSendPanel);
        }

        public void CloseSendPanel()
        {
            SendAssetView.SetActive(false);
            PlayerActions.SetActive(true);
            Confirm_SA.onClick.RemoveAllListeners();
            _currentAsset = null;
        }

        public void OpenSendPanel(Sprite sprite, PlayerInventoryResponse_Result asset)
        {
            SendAssetView.SetActive(true);
            PlayerActions.SetActive(false);
            AssetView.sprite = sprite;
            AssetName.text = asset.Name;
            _currentAsset = asset;

            Confirm_SA.onClick.AddListener(() => SendToPlayerAsync());
        }

        private async void SendToPlayerAsync()
        {
            Buffering.SetActive(true);
            await Core.SendAssetToUser("email", FriendEmail.text, _currentAsset.Id, _currentAsset.TokenId.ToString());
            Buffering.SetActive(false);
        }

        public void ShowCredentials()
        {
            CredentialsView.SetActive(true);
            AuthenticationView.SetActive(false);
        }

        public void HideCredentials()
        {
            CredentialsView.SetActive(false);
            AuthenticationView.SetActive(true);
        }

        public async void Login()
        {
            CredentialsView.SetActive(false);
            Buffering.SetActive(true);
            var s = await Auth.PlayerCustomLogin(UserName.text, Password.text);
            Buffering.SetActive(false);
            ConsoleText.text = s.ToJson();
            AuthenticationView.SetActive(false);
            PlayerActions.SetActive(true);
        }

        public async void GetPlayerInfo()
        {
            PlayerInventoryView.SetActive(false);

            Buffering.SetActive(true);
            PlayerInfoView.SetActive(false);
            var playerInfo = await Core.GetPlayerInfo();
            Buffering.SetActive(false);
            PlayerInfoView.SetActive(true);
            ConsoleText.text = playerInfo.ToJson();
            PlayerName.text = playerInfo.Data.Name;
            Email.text = playerInfo.Data.Email;
            Wallet.text = playerInfo.Data.Wallets.Evm.Address;
        }

        public async void GetPlayerInventory()
        {
            PlayerInfoView.SetActive(false);
            PlayerInventoryView.SetActive(true);

            Buffering.SetActive(true);
            var playerInventory = await Xternity.Core.GetPlayerInventory();
            Buffering.SetActive(false);
            ConsoleText.text = playerInventory.ToJson();

            foreach (var item in NFTViews)
            {
                Destroy(item);
            }

            NFTViews.Clear();

            foreach (var item in playerInventory.Data.Results)
            {
                var asset = Instantiate(NFTView_Prefab, PlayerInventoryView.transform);
                NFTViews.Add(asset.gameObject);
                asset.Init(item.Name, item.ImageUrl.ToString());
                asset.SendToFriend.onClick.AddListener(() => OpenSendPanel(asset.Presentor.sprite, item));
            }
        }

#if UNITY_EDITOR

        [ContextMenu("Fill DBD Creds")]
        public void FillCreds()
        {
            UserName.text = "dor@xternity.games";
            Password.text = "xg1234";
        }    


#endif
    }
}