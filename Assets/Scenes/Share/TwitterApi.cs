using System;
using UnityEngine;

namespace Scenes.Share
{
    public static class TwitterApi
    {
        [Serializable]
        public class OAuthResultObject
        {
            public Credential credential;
            public static OAuthResultObject FromJson(string json)
            {
                return JsonUtility.FromJson<OAuthResultObject>(json);
            }
        }

        [Serializable]
        public class Credential
        {
            public String oauthAccessToken;
            public String oauthTokenSecret;
        }
    }
}