using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _poolCapaciti = 95;
    [SerializeField] private int _poolMaxSize = 100;

    private ObjectPool<T> _pool;

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => SetActive(obj),
        actionOnRelease: (obj) => obj.gameObject.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapaciti,
        maxSize: _poolMaxSize);
    }

    protected internal T CreateGameObject()
    {
        _pool.Get(out T gameObject);

        return gameObject;
    }

    protected virtual void SetActive(T obj)
    {
        obj.gameObject.SetActive(true);

        Init(obj);
    }

    protected virtual void Init(T gameObject) { }

    protected void ReleaseGameObject(T gameObject)
    {
        _pool.Release(gameObject);
    }
}
