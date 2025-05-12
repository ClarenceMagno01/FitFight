using System;
using System.Collections.Generic;
using _Project.Scripts.Main.Utilities.Scripts.Singleton;
using UnityEngine;

namespace _Project.Scripts.Main.Utilities.Scripts
{
    [Serializable]
    public class PrefabInfo
    {
        public GameObject prefab;
        public int amount;
    }

    public class PrefabPoolManager : SingletonMonoBehaviour<PrefabPoolManager>
    {
        public List<PrefabInfo> prefabList;
        private Dictionary<string, Queue<GameObject>> _objectPoolDictionary;
        
        private new void Awake()
        {
            InitializeObjectPool();
            base.Awake();
        }

        private void InitializeObjectPool()
        {
            _objectPoolDictionary = new Dictionary<string, Queue<GameObject>>();
            var tr = transform;
            foreach (var prefabInfo in prefabList)
            {
                var objectPool = new Queue<GameObject>();

                for (var i = 0; i < prefabInfo.amount; i++)
                {
                    var obj = Instantiate(prefabInfo.prefab,tr);
                    obj.name = prefabInfo.prefab.name;
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _objectPoolDictionary.Add(prefabInfo.prefab.name, objectPool);
            }
        }

        public GameObject GetObjectFromPool(GameObject prefab)
        {
            return GetObjectFromPool(prefab, Vector3.zero, Quaternion.identity);
        }
        
        public GameObject GetObjectFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject obj = GetObjectFromPool(prefab, position, rotation);
            obj.transform.SetParent(parent);
            return obj;
        }
        
        public GameObject GetObjectFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (_objectPoolDictionary.ContainsKey(prefab.name))
            {
                if (_objectPoolDictionary[prefab.name].Count > 0)
                {
                    var obj = _objectPoolDictionary[prefab.name].Dequeue();
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                    obj.SetActive(true);
                    return obj;
                }

                var newObj = Instantiate(prefab,transform);
                newObj.transform.position = position;
                newObj.transform.rotation = rotation;
                newObj.name = prefab.name;
                newObj.SetActive(true);
                return newObj;
            }

            //Debug.Log("Prefab " + prefab.name + " not found in the object pool.");
            return null;
        }

        public void ReturnObjectToPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            _objectPoolDictionary[obj.name].Enqueue(obj);
        }

        public void RemoveAllFromParent(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                ReturnObjectToPool(parent.GetChild(i).gameObject);
            }
        }
        
    }
}