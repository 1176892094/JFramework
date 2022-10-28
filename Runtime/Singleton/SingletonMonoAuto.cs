using UnityEngine;

public class SingletonMonoAuto<T> : MonoBehaviour where T : SingletonMonoAuto<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }

            return instance;
        }
    }
}