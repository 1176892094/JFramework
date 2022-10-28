using UnityEngine;

namespace JYJFramework
{
    public class BasePanel : MonoBehaviour, IPanel
    {
        [HideInInspector] public string path;
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);

        public virtual void OnCloseButtonClick() => UIManager.Instance.HidePanel(path);

        protected virtual void OnDestroy()
        {
        }
    }
}