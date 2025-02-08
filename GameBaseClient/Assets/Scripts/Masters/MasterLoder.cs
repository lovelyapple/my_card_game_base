using Cysharp.Threading.Tasks;
using Starmessage.Master;
using MessagePack;
using MessagePack.Resolvers;
using System;
using UnityEditor;
using UnityEngine;

//https://light11.hatenadiary.com/entry/2023/11/20/194245
//これをみる
public static class BinaryLoader
{
#if UNITY_EDITOR
    [MenuItem("StarMessage/Masters/Load Binary")]
#endif
    public static void Run()
    {
        RunAsync().Forget();
    }
    public static async UniTask<MemoryDatabase> RunAsync()
    {
        // MessagePackの初期化（ボイラープレート）
        var messagePackResolvers = CompositeResolver.Create(
            MasterMemoryResolver.Instance, // 自動生成されたResolver（Namespaceごとに作られる）
            GeneratedResolver.Instance,    // 自動生成されたResolver
            StandardResolver.Instance      // MessagePackの標準Resolver
        );
        var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolvers);
        MessagePackSerializer.DefaultOptions = options;

        // ロード（テスト用にAssetDatabaseを使っているが実際にはAddressableなどで）
        var path = SystemPath.MasterBinaryPath;
        var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        var binary = asset.bytes;

        await UniTask.WaitForSeconds(1); //仮に1秒待ってみる

        // MemoryDatabaseをバイナリから作成
        var memoryDatabase = new MemoryDatabase(binary);

        return memoryDatabase;
    }
}
