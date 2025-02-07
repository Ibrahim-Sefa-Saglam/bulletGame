using UnityEngine;

namespace Systems.SaveSystem
{
    public abstract class SaveData
    {
        public abstract void Save();
        
        public abstract SaveData Load();
    }
}