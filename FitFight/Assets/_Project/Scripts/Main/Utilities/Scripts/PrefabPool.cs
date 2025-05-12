using UnityEngine;
using UnityEngine.Pool;

namespace _Project.Scripts.Main.Utilities.Scripts
{
    public class PrefabPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly ObjectPool<T> _pool;

        public PrefabPool(T prefab, Transform parent = null, int defaultCapacity = 10, int maxSize = 100)
        {
            _prefab = prefab;
            _parent = parent;

            _pool = new ObjectPool<T>(
                createFunc: () => Object.Instantiate(_prefab, _parent),
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroy,
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }

        public T Get()
        {
            return _pool.Get();
        }

        public void Release(T instance)
        {
            _pool.Release(instance);
        }

        private void OnGet(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnRelease(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroy(T obj)
        {
            Object.Destroy(obj.gameObject);
        }
        

    }

}