using System;
using UnityEngine;

namespace JFramework
{
    public class BasePanel : MonoBehaviour, IPanel
    {
        public string path;
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
    }
}