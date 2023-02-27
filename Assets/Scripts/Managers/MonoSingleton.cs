using UnityEngine;

// Template that utilises Generics for Singleton Managers. Any Manager that inherits this class, will automatically inherit the Singleton pattern.
// Abstract - Not meant to create instances of. It is built as a template for other classes to inherit from.
// Abstract classes are basically an interface that allows you to provide implementation instead of just a method declaration.
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

        Initialisation();
    }

    // Abstract method - Forces inheritance for deriving class. Will receive errors in derived class if method isn't implemented.
    // Virtual - Allows for the derived class to override this method.
    protected virtual void Initialisation()
    {
        Debug.Log(typeof(T).ToString() + " is initialised!");
    }

}
