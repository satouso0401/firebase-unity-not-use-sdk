using System;
using Scenes.Share;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Authentication
{
    public class AuthWebViewScript : MonoBehaviour
    {
        private string _projectId = Config.FirebaseProjectId;
        WebViewObject webViewObject;

        void Start()
        {
            var url = $"https://{_projectId}.firebaseapp.com/auth/auth.html";
            var salt = DateTime.Now.ToString("yyyyMMddHHmm");
            var urlWithSalt = $"{url}?_={salt}"; // WebViewはキャッシュの制御が難しいのでソルトを付与して強制的にキャッシュを無効にする

            webViewObject =
                (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
            webViewObject.Init((msg) =>
            {
                TwitterScript.oAuthResult = TwitterApi.OAuthResultObject.FromJson(msg);
                GameObject.Find("Canvas/SignInButton").GetComponent<Button>().interactable = true;
                webViewObject.SetVisibility(false);
            });

            webViewObject.LoadURL(urlWithSalt);
            webViewObject.SetMargins(50, 100, 50, 50);
            webViewObject.SetVisibility(true);
        }
    }
}