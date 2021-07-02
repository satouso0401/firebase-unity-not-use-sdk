using System;
using UnityEngine;

namespace Scenes.Share
{
    public static class FirebaseApi
    {
        [Serializable]
        public class EmailPasswordAuthRequest
        {
            public string email;
            public string password;
            public bool returnSecureToken = true;

            public EmailPasswordAuthRequest(string email, string password)
            {
                this.email = email;
                this.password = password;
            }

            public String ToJson()
            {
                return JsonUtility.ToJson(this);
            }
        }

        [Serializable]
        public class EmailPasswordAuthResponse
        {
            public string idToken;
            public string email;
            public string refreshToken;
            public string expiresIn; // トークンの有効期間（秒）
            public string localId; // uid
            public bool registered;

            public static EmailPasswordAuthResponse FromJson(string json)
            {
                return JsonUtility.FromJson<EmailPasswordAuthResponse>(json);
            }
        }

        [Serializable]
        public class SignInWithIdpRequest
        {
            public string postBody;
            public string requestUri;
            public bool returnIdpCredential;
            public bool returnSecureToken;

            public SignInWithIdpRequest(string postBody, string requestUri, bool returnIdpCredential,
                bool returnSecureToken)
            {
                this.postBody = postBody;
                this.requestUri = requestUri;
                this.returnIdpCredential = returnIdpCredential;
                this.returnSecureToken = returnSecureToken;
            }

            public static SignInWithIdpRequest SignInWithIdpTwitterRequest(string accessToken, string oauthTokenSecret,
                string requestUri = "http://localhost", bool returnIdpCredential = true, bool returnSecureToken = true)
            {
                return new SignInWithIdpRequest(
                    $"access_token={accessToken}&oauth_token_secret={oauthTokenSecret}&providerId=twitter.com",
                    requestUri,
                    returnIdpCredential,
                    returnSecureToken
                );
            }

            public String ToJson()
            {
                return JsonUtility.ToJson(this);
            }
        }
        
        [Serializable]
        public class SignInWithIdpResponse
        {
            public string providerId;
            public string localId; // uid
            public string idToken;
            public string refreshToken;
            public string expiresIn; // トークンの有効期間（秒）

            public static SignInWithIdpResponse FromJson(string json)
            {
                return JsonUtility.FromJson<SignInWithIdpResponse>(json);
            }
        }
        
        [Serializable]
        public class SignInWithCustomTokenRequest
        {
            public string token;
            public bool returnSecureToken = true;

            public SignInWithCustomTokenRequest(string token)
            {
                this.token = token;
            }
            public String ToJson()
            {
                return JsonUtility.ToJson(this);
            }
        }
        
        [Serializable]
        public class SignInWithCustomTokenResponse
        {
            public string idToken;
            public string refreshToken;
            public string expiresIn; // The number of seconds in which the ID token expires.
            
            public static SignInWithCustomTokenResponse FromJson(string json)
            {
                return JsonUtility.FromJson<SignInWithCustomTokenResponse>(json);
            }
        }

    }
}