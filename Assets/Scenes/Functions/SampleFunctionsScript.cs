using System;
using System.Net;
using Scenes.Share;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Scenes.Functions
{
    public class SampleFunctionsScript : MonoBehaviour
    {
        public void StrongEchoOnRequest()
        {
            var inputMessage = GameObject.Find("InputMessage").GetComponent<InputField>().text;
            var echo = PostStrongEchoOnRequest(inputMessage, getIdToken());
            var resultText = GameObject.Find("ResultText").GetComponent<Text>();
            resultText.text = echo;
        }

        public void StrongEchoOnCall()
        {
            var inputMessage = GameObject.Find("InputMessage").GetComponent<InputField>().text;
            var echo = PostStrongEchoOnCall(inputMessage, getIdToken());
            var resultText = GameObject.Find("ResultText").GetComponent<Text>();
            resultText.text = echo;
        }


        [Serializable]
        class StrongEchoOnRequestRequest
        {
            public string message;
            public string idToken;

            public StrongEchoOnRequestRequest(string message, string idToken)
            {
                this.message = message;
                this.idToken = idToken;
            }

            public String ToJson()
            {
                return JsonUtility.ToJson(this);
            }
        }

        [Serializable]
        class StrongEchoOnRequestResponse
        {
            public string echoMessage;

            public static StrongEchoOnRequestResponse FromJson(string json)
            {
                return JsonUtility.FromJson<StrongEchoOnRequestResponse>(json);
            }
        }

        string PostStrongEchoOnRequest(string message, string idToken)
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            var requestJson = new StrongEchoOnRequestRequest(message, idToken).ToJson();
            var url = "https://asia-northeast1-fb-vanilla-sample.cloudfunctions.net/strongEchoOnRequest";
            var responseJson = wc.UploadString(new Uri(url), requestJson);
            var response = JsonUtility.FromJson<StrongEchoOnRequestResponse>(responseJson);
            return response.echoMessage;
        }


        [Serializable]
        class StrongEchoOnCallRequest
        {
            public StrongEchoOnCallRequestData data;

            public StrongEchoOnCallRequest(string message)
            {
                this.data = new StrongEchoOnCallRequestData(message);
            }

            public String ToJson()
            {
                return JsonUtility.ToJson(this);
            }
        }

        [Serializable]
        class StrongEchoOnCallRequestData
        {
            public string message;

            public StrongEchoOnCallRequestData(string message)
            {
                this.message = message;
            }

            public String ToJson()
            {
                return JsonUtility.ToJson(this);
            }
        }

        [Serializable]
        class StrongEchoOnCallResponse
        {
            public StrongEchoOnCallResponseResult result;

            public static StrongEchoOnCallResponse FromJson(string json)
            {
                return JsonUtility.FromJson<StrongEchoOnCallResponse>(json);
            }
        }

        [Serializable]
        class StrongEchoOnCallResponseResult
        {
            public string echoMessage;
        }


        string PostStrongEchoOnCall(string message, string idToken)
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {idToken}";
            var url = $"https://asia-northeast1-{Config.FirebaseProjectId}.cloudfunctions.net/strongEchoOnCall";
            var requestJson = new StrongEchoOnCallRequest(message).ToJson();
            var responseJson = wc.UploadString(new Uri(url), requestJson);
            var response = JsonUtility.FromJson<StrongEchoOnCallResponse>(responseJson);
            return response.result.echoMessage;
        }

        string getIdToken()
        {
            // Note: サンプル実装として簡単のため毎回新しいメールでサインアップしています。認証方法は以下のURLを参考にしてください。
            // https://zenn.dev/satouso/articles/c648d250553170#%E8%AA%8D%E8%A8%BC-(firebase-authentication)
            var rand = new Random();
            var email = $"sample{rand.Next(0, 10000)}@example.com";
            var password = "foobar" + rand.Next(0, 10000);
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={Config.FirebaseApiKey}";
            var requestBody = new FirebaseApi.EmailPasswordAuthRequest(email, password).ToJson();
            var response = wc.UploadString(new Uri(url), requestBody);
            var authResult = FirebaseApi.EmailPasswordAuthResponse.FromJson(response);
            return authResult.idToken;
        }
    }
}