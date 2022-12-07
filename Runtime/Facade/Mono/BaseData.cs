using UnityEngine;

namespace JFramework.Basic
{
    public abstract class BaseData : ScriptableObject, IData
    {
        public virtual void InitData()
        {
        }

        public virtual void SaveData()
        {
        }

        public virtual void LoadData()
        {
        }
    }
    
    public interface IData
    {
        void InitData();
        void SaveData();
        void LoadData();
    }
}