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


    }
}