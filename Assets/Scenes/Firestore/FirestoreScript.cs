using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Scenes.Share;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Scenes.Firestore
{
    public class FirestoreScript : MonoBehaviour
    {
        private static readonly string ProjectId = Config.FirebaseProjectId;
        private static readonly string APIKey = Config.FirebaseApiKey;
        private static readonly (string, string) UidWithIdToken = getUidWithIdToken();
        private static readonly string Uid = UidWithIdToken.Item1;
        private static readonly string IDToken = UidWithIdToken.Item2;


        [Serializable]
        public class PageFields
        {
            public FirestoreApi.StringField title;
            public FirestoreApi.IntegerField memoNo;
            public FirestoreApi.StringField text;

            public PageFields(FirestoreApi.StringField title, FirestoreApi.IntegerField memoNo,
                FirestoreApi.StringField text)
            {
                this.title = title;
                this.memoNo = memoNo;
                this.text = text;
            }
        }

        [Serializable]
        public class MemoCreateDocumentRequest
        {
            public PageFields fields;

            public MemoCreateDocumentRequest(PageFields fields)
            {
                this.fields = fields;
            }

            public static MemoCreateDocumentRequest Apply(string title, int memoNo, string text)
            {
                return new MemoCreateDocumentRequest(
                    new PageFields(
                        new FirestoreApi.StringField(title),
                        new FirestoreApi.IntegerField(memoNo),
                        new FirestoreApi.StringField(text)
                    )
                );
            }

            public String ToJson()
            {
                return JsonUtility.ToJson(this);
            }
        }


        [Serializable]
        public class MemoDocument
        {
            public string name;
            public PageFields fields;
        }

        [Serializable]
        public class MemoRunQueryResponse
        {
            public MemoDocument document;
        }

        [Serializable]
        public class MemoRunQueryResponseWrapper
        {
            public MemoRunQueryResponse[] array;

            public static MemoRunQueryResponse[] FromJson(string json)
            {
                var wrapper = JsonUtility.FromJson<MemoRunQueryResponseWrapper>($"{{\"array\": {json}}}");
                return wrapper.array;
            }
        }


        public void CreateDocument()
        {
            // メモに登録する内容
            var title = GameObject.Find("TitleInput").GetComponent<InputField>().text;
            var memoNo = Convert.ToInt32(GameObject.Find("MemoNoInput").GetComponent<InputField>().text);
            var text = GameObject.Find("TextInput").GetComponent<InputField>().text;

            // 認証情報の設定
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {IDToken}";

            // ドキュメントの保存先の指定
            var url =
                $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/users/{Uid}/memos";

            // 保存するドキュメントの内容のjson化
            var requestBody = JsonUtility.ToJson(MemoCreateDocumentRequest.Apply(title, memoNo, text));
            Debug.Log(requestBody);

            // 登録処理
            var responseJson = wc.UploadString(new Uri(url), requestBody);
            Debug.Log(responseJson);
        }


        // https://firebase.google.com/docs/firestore/reference/rest#rest-resource:-v1.projects.databases.documents

        public void RunQuery()
        {
            // 認証情報の設定
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.Headers[HttpRequestHeader.Authorization] = $"Bearer {IDToken}";

            // 検索対象のコレクションを指定
            var url =
                $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/users/{Uid}:runQuery";

            // 検索条件の指定
            var requestBody = new FirestoreApi.Query("memos").ToJson();
            Debug.Log(requestBody);

            // 検索結果
            var responseJson = wc.UploadString(new Uri(url), requestBody);
            Debug.Log(responseJson);

            // 検索結果の表示
            var memos = MemoRunQueryResponseWrapper.FromJson(responseJson);
            var titleList = memos.Aggregate("", (acc, memo) => acc + memo.document.fields.title.stringValue + "\n");
            var memoList = GameObject.Find("MemoList").GetComponent<Text>();
            memoList.text = titleList;
        }


        static (string, string) getUidWithIdToken()
        {
            // 事前に以下のユーザーをFirebase Authenticationに登録してください
            var email = "test.user@example.com";
            var password = "foobar1234";

            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={APIKey}";
            var requestBody = new FirebaseApi.EmailPasswordAuthRequest(email, password).ToJson();
            var response = wc.UploadString(new Uri(url), requestBody);
            var authResult = FirebaseApi.EmailPasswordAuthResponse.FromJson(response);
            return (authResult.localId, authResult.idToken);
        }
    }
}