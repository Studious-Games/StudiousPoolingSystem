using UnityEngine;
using Studious.Pooling;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _spawnPosition;

    private ObjectPool<Bullet> _objectPool;

    //-----------------------------------------------------------------------------
    //
    //-----------------------------------------------------------------------------
    private void Awake()
    {
        _objectPool = new ObjectPool<Bullet>(_bullet, 10);

    }

    //-----------------------------------------------------------------------------
    //
    //-----------------------------------------------------------------------------
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _objectPool.Pull(_spawnPosition.position, _spawnPosition.transform.parent.rotation);
        }

    }
}
