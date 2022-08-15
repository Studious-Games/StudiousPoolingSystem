using System;
using System.Collections;
using UnityEngine;

using Studious.Pooling;

public class Bullet : MonoBehaviour, IPoolable<Bullet>
{
    [SerializeField] private float _aliveTime;
    [SerializeField] private float _movementSpeed;

    private WaitForSeconds _destroyTime;
    private Action<Bullet> _returnToPool;

    //-----------------------------------------------------------------------------
    //
    //-----------------------------------------------------------------------------
    public void Initialize(Action<Bullet> returnAction)
    {
        _returnToPool = returnAction;
        StartCoroutine(KeepAlive());
    }

    //-----------------------------------------------------------------------------
    //
    //-----------------------------------------------------------------------------
    public void ReturnToPool()
    {
        _returnToPool?.Invoke(this);
    }

    //-----------------------------------------------------------------------------
    //
    //-----------------------------------------------------------------------------
    private void Awake()
    {
        _destroyTime = new WaitForSeconds(5.0f);
    }

    //-----------------------------------------------------------------------------
    //
    //-----------------------------------------------------------------------------
    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * _movementSpeed;
    }

    //-----------------------------------------------------------------------------
    //
    //-----------------------------------------------------------------------------
    private IEnumerator KeepAlive()
    {
        yield return _destroyTime;
        ReturnToPool();
    }
}
