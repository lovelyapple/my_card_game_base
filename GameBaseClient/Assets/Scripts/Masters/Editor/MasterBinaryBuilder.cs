using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Starmessage.Master;
using MasterMemory;
using System.Diagnostics;
// https://light11.hatenadiary.com/entry/2023/12/19/193216#Csv%E3%81%8B%E3%82%89%E3%83%90%E3%82%A4%E3%83%8A%E3%83%AA%E3%82%92%E4%BD%9C%E3%82%8B
public sealed class MasterBinaryBuilder
{
    public void Read(DatabaseBuilder builder, string tsv, string tableName)
    {
        var metaDatabase = MemoryDatabase.GetMetaDatabase();
        var table = metaDatabase.GetTableInfo(tableName);
        var records = new List<object>();

        using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(tsv)))
        using (var streamReader = new StreamReader(memoryStream, Encoding.UTF8))
        using (var tsvReader = new TinyTsvReader(streamReader))
        {
            // Tsvを一行ずつ読むループ
            while (true)
            {
                var columnNameToValueMap = tsvReader.ReadValuesWithHeader();

                // これ以上TSVにデータがなかったら終了
                if (columnNameToValueMap == null)
                    break;

                // 各プロパティの名前に一致するカラムを探して値を設定する（一致するカラムがないものはスキップ）
                var record = FormatterServices.GetUninitializedObject(table.DataType);
                foreach (var property in table.Properties)
                {
                    var columnName = property.Name;
                    if (!columnNameToValueMap.TryGetValue(columnName, out var rawValue))
                    {
                        UnityEngine.Debug.LogError($"columnName :{columnName} not found  in master:{tableName}");
                        continue;
                    }

                    var value = ParseValue(property.PropertyInfo.PropertyType, rawValue);
                    if (property.PropertyInfo.SetMethod == null)
                    {
                        var message =
                            $"Target property does not exists set method. If you use {{get;}}, please change to {{ get; private set; }}, Type: {property.PropertyInfo.DeclaringType} Prop: {property.PropertyInfo.Name}";
                        throw new Exception(message);
                    }

                    property.PropertyInfo.SetValue(record, value);
                }

                records.Add(record);
            }
        }

        // AppendDynamicでレコードを追加する
        builder.AppendDynamic(table.DataType, records);
    }
    static string ConvertToSnakeCase(string input)
    {
        string result = "";

        foreach (char c in input)
        {
            if (char.IsUpper(c))
            {
                if (result.Length > 0)
                {
                    result += "_";
                }
                result += char.ToLower(c);
            }
            else
            {
                result += c;
            }
        }

        return result;
    }
    public byte[] Build(DatabaseBuilder builder)
    {
        return builder.Build();
    }

    #region この部分はMasterMemoryのドキュメントのまま

    private static object ParseValue(Type type, string rawValue)
    {
        if (type == typeof(string)) return rawValue;

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            if (string.IsNullOrWhiteSpace(rawValue)) return null;
            return ParseValue(type.GenericTypeArguments[0], rawValue);
        }

        if (type.IsEnum)
        {
            var value = Enum.Parse(type, rawValue);
            return value;
        }

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Boolean:
                // True/False or 0,1
                if (int.TryParse(rawValue, out var intBool)) return Convert.ToBoolean(intBool);
                return bool.Parse(rawValue);
            case TypeCode.Char:
                return char.Parse(rawValue);
            case TypeCode.SByte:
                return sbyte.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.Byte:
                return byte.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.Int16:
                return short.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.UInt16:
                return ushort.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.Int32:
                return int.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.UInt32:
                return uint.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.Int64:
                return long.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.UInt64:
                return ulong.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.Single:
                return float.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.Double:
                return double.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.Decimal:
                return decimal.Parse(rawValue, CultureInfo.InvariantCulture);
            case TypeCode.DateTime:
                return DateTime.Parse(rawValue, CultureInfo.InvariantCulture);
            default:
                if (type == typeof(DateTimeOffset))
                    return DateTimeOffset.Parse(rawValue, CultureInfo.InvariantCulture);
                if (type == typeof(TimeSpan))
                    return TimeSpan.Parse(rawValue, CultureInfo.InvariantCulture);
                if (type == typeof(Guid)) return Guid.Parse(rawValue);

                // or other your custom parsing.
                throw new NotSupportedException();
        }
    }

    // Non string escape, tiny reader with header.
    public class TinyTsvReader : IDisposable
    {
        private static readonly char[] trim = { ' ', '\n' };

        private readonly StreamReader reader;

        public TinyTsvReader(StreamReader reader)
        {
            this.reader = reader;
            {
                var line = reader.ReadLine();
                if (line == null) throw new InvalidOperationException("Header is null.");

                var index = 0;
                var header = new List<string>();
                while (index < line.Length)
                {
                    var s = GetValue(line, ref index);
                    if (s.Length == 0) break;
                    header.Add(s);
                }

                Header = header;
            }
        }

        public IReadOnlyList<string> Header { get; }

        public void Dispose()
        {
            reader.Dispose();
        }

        private string GetValue(string line, ref int i)
        {
            var temp = new char[line.Length - i];
            var j = 0;
            for (; i < line.Length; i++)
            {
                if (line[i] == '\t')
                {
                    i += 1;
                    break;
                }

                temp[j++] = line[i];
            }

            return new string(temp, 0, j).Trim(trim);
        }

        public string[] ReadValues()
        {
            var line = reader.ReadLine();
            if (line == null) return null;
            if (string.IsNullOrWhiteSpace(line)) return null;

            var values = new string[Header.Count];
            var lineIndex = 0;
            for (var i = 0; i < values.Length; i++)
            {
                var s = GetValue(line, ref lineIndex);
                values[i] = s;
            }

            return values;
        }

        public Dictionary<string, string> ReadValuesWithHeader()
        {
            var values = ReadValues();
            if (values == null) return null;

            var dict = new Dictionary<string, string>();
            for (var i = 0; i < values.Length; i++) dict.Add(Header[i], values[i]);

            return dict;
        }
    }

    #endregion
}