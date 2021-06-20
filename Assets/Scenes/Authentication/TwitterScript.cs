using System;
using System.Net;
using Scenes.Share;
using UnityEngine;

namespace Scenes.Authentication
{
    public class TwitterScript : MonoBehaviour
    {
        private string _apiKey = Config.ApiKey;
        public static TwitterApi.OAuthResultObject oAuthResult;
        public void SignIn()
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={_apiKey}";
            var requestBody = FirebaseApi.SignInWithIdpRequest.SignInWithIdpTwitterRequest(
                oAuthResult.credential.oauthAccessToken,
                oAuthResult.credential.oauthTokenSecret
            ).ToJson();
            var response = wc.UploadString(new Uri(url), requestBody);
            var authResult = FirebaseApi.SignInWithIdpResponse.FromJson(response);
            Debug.Log($"idToken: {authResult.idToken}");
            Debug.Log($"refreshToken: {authResult.refreshToken}");
        }
    }

}
