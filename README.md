# firebase-unity-not-use-sdk

このリポジトリは以下の投稿のUnity側のサンプルコード部分です。  
[UnityでSDKなしでFirebaseを使う](https://zenn.dev/satouso/articles/c648d250553170)

Firebase側  
[firebase-unity-not-use-sdk-firebase](https://github.com/satouso0401/firebase-unity-not-use-sdk-firebase)

Assets/Config.csの`FirebaseApiKey`と`FirebaseProjectId`をお使いのFirebaseのWeb管理画面のプロジェクトの設定に表示される`ウェブ API キー`と`プロジェクト ID`に変更することでサンプルコードを動かすことができます。

```cs:Assets/Config.cs
public static class Config
{
    public const string FirebaseApiKey = "change me";
    public const string FirebaseProjectId = "change me";
}
```

