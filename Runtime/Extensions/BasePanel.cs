using UnityEngine;

namespace JFramework.Basic
{
    public class BasePanel : MonoBehaviour
    {
        public string path;
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
    }
}