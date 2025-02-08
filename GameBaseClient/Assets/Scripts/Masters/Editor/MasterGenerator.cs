using System.IO;
using MessagePack;
using MessagePack.Resolvers;
using UnityEditor;
using Starmessage.Master;

public static class BinaryGenerator
{
    [MenuItem("StarMessage/Masters/Generate Binary Manual")]
    private static void RunManual()
    {
        // // MessagePackの初期化（ボイラープレート）
        // var messagePackResolvers = CompositeResolver.Create(
        //     MasterMemoryResolver.Instance, // 自動生成されたResolver（Namespaceごとに作られる）
        //     GeneratedResolver.Instance,    // 自動生成されたResolver
        //     StandardResolver.Instance      // MessagePackの標準Resolver
        // );
        // var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolvers);
        // MessagePackSerializer.DefaultOptions = options;

        // // Csvとかからデータを入れる（今回はテストのためコードで入れる）
        // var stageMasters = new StageMaster[]
        //     {
        //     new("stage-01-001", "初心者の森"),
        //     new("stage-01-002", "迷いの湿地帯"),
        //     new("stage-01-003", "炎の山脈"),
        //     new("stage-02-001", "氷結の洞窟"),
        //     new("stage-02-002", "幽霊の墓地"),
        //     new("stage-02-003", "竜の城塞"),
        //     };

        // // DatabaseBuilderを使ってバイナリデータを生成する
        // var databaseBuilder = new DatabaseBuilder();
        // databaseBuilder.Append(stageMasters);
        // var binary = databaseBuilder.Build();

        // // できたバイナリは永続化しておく
        // var path = SystemPath.MasterBinaryPath;
        // var directory = Path.GetDirectoryName(path);
        // if (!Directory.Exists(directory))
        //     Directory.CreateDirectory(directory);
        // File.WriteAllBytes(path, binary);
        // AssetDatabase.Refresh();
    }
    [MenuItem("StarMessage/Masters/Generate Binary tsv")]
    private static void RunTsv()
    {
        // MessagePackの初期化（ボイラープレート）
        var messagePackResolvers = CompositeResolver.Create(
            MasterMemoryResolver.Instance,
            GeneratedResolver.Instance,
            StandardResolver.Instance
        );
        var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolvers);
        MessagePackSerializer.DefaultOptions = options;
        // バイナリを作成
        var databaseBanaryBuilder = new MasterBinaryBuilder();
        var memoryDataBaseBuilder = new DatabaseBuilder();
        foreach (var tableName in SystemPath.TablePathList)
        {
            var dataPath = $"{SystemPath.MasterTsvRootPath}{tableName}.tsv";
            var tsv = ReadFile(dataPath);
            databaseBanaryBuilder.Read(memoryDataBaseBuilder, tsv, tableName);
        }

        var binary = databaseBanaryBuilder.Build(memoryDataBaseBuilder);
        // バイナリを永続化
        var path = SystemPath.MasterBinaryPath;
        var directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllBytes(path, binary);
    }
    private static string ReadFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string result = "";
            int lineNumber = 0;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                lineNumber++;

                // 2行目はスキップ
                if (line.StartsWith("#"))
                {
                    continue;
                }

                // 必要ならデータを結合 (例: 改行を含めて保持)
                result += line + "\n";
            }

            return result.TrimEnd(); // 最後の余分な改行を削除
        }
    }
}
