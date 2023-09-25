using UnityEngine;

// Template that utilises Generics for Singleton Managers. Any Manager that inherits this class, will automatically inherit the Singleton pattern.
// Abstract - Not meant to create instances of. It is built as a template for other classes to inherit from.
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    #region MONOSINGLETON
    private static T _instance;
    public static T Instance
    {
        get 
        {
            if( _instance == null ) 
                Debug.Log(typeof(T).ToString() + " is NULL!");
            return _instance;
        }
    }
    #endregion

    void Awake()
    {
        _instance = this as T;
        // Or,
        //_instance = (T)this;
    }
}
