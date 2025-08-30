using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for managing the singleton classes.
/// If you want to make any class singleton, iherit from this class.
/// Note: Clears the instance when a scene is changed.
/// </summary>
public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;

    /// <summary>
    /// The instance object of this class (Static only)
    /// </summary>
    ///

    //public static T Instance
    //{
    //    get
    //    {
    //        return (T)_singletons[typeof(T)];
    //    }
    //}

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //_instance = this.getco;
                _instance = GameObject.FindObjectOfType<T>();
                //_instance = this.gameobject;
            }

            if (_instance == null)
            {
                //Debug.LogWarning("Again instance is NULL.. " + typeof(T).Name);
            }

            return _instance;
        }
    }

    /// <summary>
    /// Indicates whether this class has a valid instance. 
    /// Note : Use this to validate the instance of this class
    /// </summary>
    public static bool HasInstance
    {
        get
        { 

            if (_instance == null)
            {
                Debug.LogWarning("Warning - No instance found " + _instance.GetType().ToString());
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Clears the instance when the object is Destroyed when a scene changes or if it is destroyed.
    /// </summary>
    void OnDestroy()
    {
        _instance = null;
    }
}
