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
    }
}