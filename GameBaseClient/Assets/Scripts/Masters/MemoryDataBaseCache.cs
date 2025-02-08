using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Starmessage.Master
{
    public class MemoryDataBaseCache
    {
        private static MemoryDataBaseCache _instace;
        public static MemoryDataBaseCache Instance()
        {
            if (_instace == null)
            {
                _instace = new MemoryDataBaseCache();
            }

            return _instace;
        }
        private MemoryDatabase _memoryDatabase;
        public static MemoryDatabase TableCache
        {
            get { return _instace._memoryDatabase; }
        }
        public async UniTask InitializeAsync()
        {
            _memoryDatabase = await BinaryLoader.RunAsync();
        }
    }
}
