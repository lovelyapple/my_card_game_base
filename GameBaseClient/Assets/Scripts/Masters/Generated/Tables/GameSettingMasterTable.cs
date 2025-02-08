// <auto-generated />
#pragma warning disable CS0105
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;

namespace Starmessage.Master.Tables
{
   public sealed partial class GameSettingMasterTable : TableBase<GameSettingMaster>, ITableUniqueValidate
   {
        public Func<GameSettingMaster, string> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<GameSettingMaster, string> primaryIndexSelector;


        public GameSettingMasterTable(GameSettingMaster[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Key;
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();


        public GameSettingMaster FindByKey(string key)
        {
            return FindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, true);
        }
        
        public bool TryFindByKey(string key, out GameSettingMaster result)
        {
            return TryFindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, out result);
        }

        public GameSettingMaster FindClosestByKey(string key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<GameSettingMaster> FindRangeByKey(string min, string max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.StringComparer.Ordinal, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "Key", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(GameSettingMaster), typeof(GameSettingMasterTable), "game_setting_master",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(GameSettingMaster).GetProperty("Key")),
                    new MasterMemory.Meta.MetaProperty(typeof(GameSettingMaster).GetProperty("Value1")),
                    new MasterMemory.Meta.MetaProperty(typeof(GameSettingMaster).GetProperty("Value2")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(GameSettingMaster).GetProperty("Key"),
                    }, true, true, System.StringComparer.Ordinal),
                });
        }

#endif
    }
}