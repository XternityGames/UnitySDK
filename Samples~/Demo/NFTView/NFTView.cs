using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Xternity.Samples
{
    public class NFTView : MonoBehaviour
    {
        public TMP_Text Name;
        public Animator Animator;
        public Image Presentor;
        public Button SendToFriend;

        private UnityWebRequest _uwr;

        public void Init(string name, string uri)
        {
            Name.text = name;
            _uwr = UnityWebRequestTexture.GetTexture(uri);
            _uwr.SendWebRequest().completed += NFTView_completed;
        }

        private void NFTView_completed(AsyncOperation op)
        {
            Animator.enabled = false;
            Texture2D download = ((DownloadHandlerTexture)_uwr.downloadHandler).texture;
            Presentor.sprite = Sprite.Create(download, new Rect(0.0f, 0.0f, download.width, download.height), new Vector2(0.5f, 0.5f), 100.0f);
            _uwr.Dispose();
        }
    }
}
