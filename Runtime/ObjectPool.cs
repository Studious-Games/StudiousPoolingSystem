using System.Collections.Generic;
using System;
using UnityEngine;

namespace Studious.Pooling
{

    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private Action<T> _pullAction;
        private Action<T> _pushAction;
        private Stack<T> _pooledObjects = new Stack<T>();
        private T _prefab;
        private GameObject _prefabParent;

        public int pooledCount
        {
            get { return _pooledObjects.Count; }
        }

        /// <summary>
        /// Setup and Object pool to hold objects/componets to be used instead
        /// of instantiating and destrying them.
        /// </summary>
        /// <param name="pooledObject">The Object/Component to pool.</param>
        /// <param name="numToSpawn">Initialise the pool with this quantity.</param>
        public ObjectPool(T pooledObject, int numToSpawn = 0)
        {
            _prefab = pooledObject;
            CreateObjects(numToSpawn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pooledObject"></param>
        /// <param name="pullAction"></param>
        /// <param name="pushAction"></param>
        /// <param name="numToSpawn"></param>
        public ObjectPool(T pooledObject, Action<T> pullAction, Action<T> pushAction, int numToSpawn = 0)
        {
            _prefab = pooledObject;
            _pullAction = pullAction;
            _pushAction = pushAction;
            CreateObjects(numToSpawn);
        }

        /// <summary>
        /// Grab an object from the pool.
        /// </summary>
        /// <returns>retusn the first free object.</returns>
        public T Pull()
        {
            T t;
            if (pooledCount > 0)
                t = _pooledObjects.Pop();
            else
            {
                t = CreateObject(); //MonoBehaviour.Instantiate(_prefab);
                t.transform.parent = _prefabParent.transform;
            }

            t.gameObject.SetActive(true);
            t?.Initialize(Push);
            _pullAction?.Invoke(t);

            return t;
        }

        /// <summary>
        /// Grab an object from the pool.
        /// </summary>
        /// <param name="position">Set the positon for the object.</param>
        /// <returns>an object from the pool.</returns>
        public T Pull(Vector3 position)
        {
            T t = Pull();
            t.transform.position = position;
            return t;
        }

        /// <summary>
        /// Grab an object from the pool.
        /// </summary>
        /// <param name="position">Set the positon for the object.</param>
        /// <param name="rotation">Set the rotation for the oebject.</param>
        /// <returns>an object from the pool.</returns>
        public T Pull(Vector3 position, Quaternion rotation)
        {
            T t = Pull();
            t.transform.position = position;
            t.transform.rotation = rotation;
            return t;
        }

        /// <summary>
        /// put an objec back into the pool.
        /// </summary>
        /// <param name="t">the object to place back in the pool.</param>
        public void Push(T t)
        {
            _pooledObjects.Push(t);
            _pushAction?.Invoke(t);
            t.gameObject.SetActive(false);
        }

        private void CreateObjects(int number)
        {
            _prefabParent = new GameObject($"ObjectPool<{_prefab.name}>");

            for (int i = 0; i < number; i++)
            {
                T obj = CreateObject();
                _pooledObjects.Push(obj);
            }
        }

        private T CreateObject()
        {
            T obj = MonoBehaviour.Instantiate(_prefab);
            obj.gameObject.SetActive(false);
            obj.transform.parent = _prefabParent.transform;
            return obj;
        }
    }

    public interface IPool<T>
    {
        T Pull();
        void Push(T t);
    }

}
