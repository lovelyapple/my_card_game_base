using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
public static class BashCommandInputter
{
    public static async UniTask<string> DoBashCommand(string cmd)
    {
        var p = new Process();
        p.StartInfo.FileName = SystemPath.BashCommandBinPath;
        p.StartInfo.Arguments = "-c \" " + cmd + " \"";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
#if !UNITY_STANDALONE_WIN
        // 既存のPATHに dotnet と mpc のパスを追加
        string existingPath = System.Environment.GetEnvironmentVariable("PATH");
        string homePath = System.Environment.GetEnvironmentVariable("HOME");

        // dotnet と mpc のパスを追加
        string additionalPaths = $"/usr/local/share/dotnet:{homePath}/.dotnet/tools";

        // 統合して環境変数に設定
        p.StartInfo.EnvironmentVariables["PATH"] = existingPath + ":" + additionalPaths;
#endif

        p.Start();

        var output = await p.StandardOutput.ReadToEndAsync();
        p.WaitForExit();
        p.Close();

        return output;
    }
}
