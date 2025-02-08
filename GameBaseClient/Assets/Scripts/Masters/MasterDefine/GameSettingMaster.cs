using MasterMemory;
using MessagePack;

// クラスにMemoryTableアトリビュートをつける（引数の文字列+MasterTableという名前のクラスが生成される）
// クラスにMessagePackObjectアトリビュートをつける
[MemoryTable("game_setting_master"), MessagePackObject(true)]
public sealed class GameSettingMaster
{
    [PrimaryKey]
    public string Key { get; set; }
    public string Value1 { get; set; }
    public string Value2 { get; set; }

    public GameSettingMaster(string Key, string Value1, string Value2)
    {
        this.Key = Key;
        this.Value1 = Value1;
        this.Value2 = Value2;
    }
}
