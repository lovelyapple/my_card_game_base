using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemPath
{
    public const string MasterMemoryClassDefinePath = "./Assets/Scripts/Masters/MasterDefine";
    public const string MasterMemoryGeneratePath = "./Assets/Scripts/Masters/Generated";
    public const string MasterBinaryPath = "Assets/ExternalResources/Masters/Binary/MasterData.bytes";
    public const string MasterTsvRootPath = "Assets/ExternalResources/Masters/";

#if UNITY_STANDALONE_WIN
    public const string BashCommandBinPath = "C:/Program Files/Git/usr/bin/bash";
#else
    public const string BashCommandBinPath = "/bin/zsh";
#endif
    public readonly static List<string> TablePathList = new List<string>()
    {
        "game_setting_master",
    };
}
