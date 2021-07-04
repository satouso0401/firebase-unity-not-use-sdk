using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Scenes.Share;
using UnityEngine;

namespace Scenes.Authentication
{
    public class RefreshIdTokenScript : MonoBehaviour
    {
        private string _apiKey = Config.FirebaseApiKey;
        private static string _refreshToken;

        public void GetRefreshToken()
        {
            var email = "assafnativ@example.com";
            var password = "dolphins";
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_apiKey}";
            var requestBody = new FirebaseApi.EmailPasswordAuthRequest(email, password).ToJson();
            var response = wc.UploadString(new Uri(url), requestBody);
            var authResult = FirebaseApi.EmailPasswordAuthResponse.FromJson(response);
            _refreshToken = authResult.refreshToken;

            Debug.Log($"GetRefreshToken idToken: {authResult.idToken}");
            Debug.Log($"GetRefreshToken refreshToken: {authResult.refreshToken}");
            Debug.Log($"GetRefreshToken expiresIn: {authResult.expiresIn}");
        }

        public void RefreshIdToken()
        {
            WebClient wc = new WebClient();
            NameValueCollection nvs = new NameValueCollection();
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var url = $"https://securetoken.googleapis.com/v1/token?key={_apiKey}";
            nvs.Add("grant_type", "refresh_token");
            nvs.Add("refresh_token", _refreshToken);
            var response = wc.UploadValues(new Uri(url), nvs);
            var idTokenResult = FirebaseApi.RefreshIdTokenResponse.FromJson(Encoding.UTF8.GetString(response));

            Debug.Log($"RefreshIdToken idToken: {idTokenResult.id_token}");
            Debug.Log($"RefreshIdToken refreshToken: {idTokenResult.refresh_token}");
            Debug.Log($"RefreshIdToken expiresIn: {idTokenResult.expires_in}");
        }
    }
}