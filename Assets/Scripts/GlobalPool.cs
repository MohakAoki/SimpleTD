using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPool : MonoBehaviour
{
    public static GlobalPool Instance;

    private List<MonoBehaviour> _gameObjectPool;

    private void Awake()
    {
        Debug.Assert(Instance == null);

        Instance = this;

        _gameObjectPool = new List<MonoBehaviour>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public T GetObject<T>() where T : MonoBehaviour
    {
        T result = null;

        for (int i = 0; i < _gameObjectPool.Count; i++)
        {
            if (_gameObjectPool[i] is T)
            {
                result = _gameObjectPool[i] as T;
                _gameObjectPool.RemoveAt(i);

                return result;
            }
        }

        // Not In Pool
        return result;
    }

    public GameObject GetObject(Type objType)
    {
        for (int i = 0; i < _gameObjectPool.Count; i++)
        {
            if (_gameObjectPool[i].GetType().Equals(objType))
            {
                GameObject obj = _gameObjectPool[i].gameObject;
                _gameObjectPool.RemoveAt(i);

                return obj;
            }
        }

        return null;
    }

    public void Pool<T>(T obj) where T : MonoBehaviour
    {
        if (obj == null)
        {
            Debug.LogWarning("Bad Input");
        }
        else
        {
            _gameObjectPool.Add(obj);
            obj.gameObject.SetActive(false);
            obj.transform.parent = transform;
        }
    }
}
