using System;
using System.Linq;
using System.Net;
using Scenes.Share;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Scenes.Firestore
{
    public static class FirestoreApi
    {
        [Serializable]
        public class StringField
        {
            public string stringValue;

            public StringField(string value)
            {
                stringValue = value;
            }
        }

        [Serializable]
        public class IntegerField
        {
            public string integerValue; // integerValueの場合は文字列。doubleValueの場合は数値。

            public IntegerField(int value)
            {
                integerValue = value.ToString();
            }
        }


        [Serializable]
        public class From
        {
            public string collectionId;

            public From(string collectionId)
            {
                this.collectionId = collectionId;
            }
        }

        [Serializable]
        public class StructuredQuery
        {
            public From[] from;
            public int? limit;

            public StructuredQuery(From[] froms, int? limit)
            {
                this.from = froms;
                this.limit = limit;
            }
        }

        [Serializable]
        public class Query
        {
            public StructuredQuery structuredQuery;

            public Query(From[] froms, int? limit = null) // FromでllDescendantsを使いたくなった場合用
            {
                structuredQuery = new StructuredQuery(froms, limit);
            }

            public Query(string collectionId, int? limit = null)
            {
                structuredQuery = new StructuredQuery(new []{new From(collectionId)}, limit);
            }            

            public String ToJson()
            {
                return JsonUtility.ToJson(this);
            }
        }
    }
}