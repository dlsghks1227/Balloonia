using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static T m_instance;

    public static T Instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = FindObjectOfType(typeof(T)) as T;
                if (null == m_instance)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    m_instance = obj.AddComponent<T>();
                }
            }

            return m_instance;
        }
    }
}

public abstract class Singleton<T> where T : class, new()
{
    static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new T();
            }
            return m_instance;
        }
    }
}