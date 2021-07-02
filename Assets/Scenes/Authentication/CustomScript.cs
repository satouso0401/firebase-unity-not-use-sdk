using System;
using System.Net;
using Scenes.Share;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Authentication
{
    public class CustomScript : MonoBehaviour
    {
        private string _apiKey = Config.ApiKey;
        
        public void CustomTokenSignin()
        {
            var inputSerialCode = GameObject.Find("InputSerialCode").GetComponent<InputField>().text;
            var resultText = GameObject.Find("ResultText").GetComponent<Text>();
            
            var customToken = GetCustomToken(inputSerialCode);
            var signInResult = SignInWithCustomToken(customToken);
            
            Debug.Log($"idToken: {signInResult.idToken}");
            Debug.Log($"refreshToken: {signInResult.refreshToken}");
            resultText.text = "サインインに成功しました";
        }
        
        [Serializable]
        class GetCustomTokenResponse
        {
            public string customToken;
        }
        
        string GetCustomToken(string serialCode)
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            var url =
                $"https://asia-northeast1-fb-vanilla-sample.cloudfunctions.net/customAuth?serialCode={serialCode}";
            var response = wc.DownloadString(new Uri(url));
            var customTokenResponse = JsonUtility.FromJson<GetCustomTokenResponse>(response);
            return customTokenResponse.customToken;
        }

        FirebaseApi.SignInWithCustomTokenResponse SignInWithCustomToken(string token)
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={_apiKey}";
            var requestBody = new FirebaseApi.SignInWithCustomTokenRequest(token);
            var json = JsonUtility.ToJson(requestBody);
            var response = wc.UploadString(new Uri(url), json);
            return JsonUtility.FromJson<FirebaseApi.SignInWithCustomTokenResponse>(response);
        }
    }
}
