using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private int _poolCapaciti = 95;
    [SerializeField] private int _poolMaxSize = 100;
    [SerializeField] private float _repeatTime = 5f;
    [SerializeField] private float _resourceAreaSize = 50;
    [SerializeField] private float _heightArea = 0;
    [SerializeField] private Base _base;

    private ObjectPool<Resource> _pool;

    public event Action<Resource> ResourceWasBorned;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => SetActive(obj),
        actionOnRelease: (obj) => obj.gameObject.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj.gameObject),
        collectionCheck: true,
        defaultCapacity: _poolCapaciti,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(RepeatGetResource(_repeatTime));
    }

    private void OnEnable()
    {
        _base.ResourceUnloaded += Release;
    }

    private void OnDisable()
    {
        _base.ResourceUnloaded -= Release;
    }

    private void CreateResource()
    {
        _pool.Get();
    }

    private void Release(Resource resource)
    {
        _pool.Release(resource);
    }

    private void SetActive(Resource resource)
    {
        resource.gameObject.SetActive(true);

        Init(resource);
    }

    private void Init(Resource resource)
    {
        resource.transform.position = new Vector3(
            UnityEngine.Random.Range(-_resourceAreaSize, _resourceAreaSize), _heightArea,
            UnityEngine.Random.Range(-_resourceAreaSize, _resourceAreaSize));

        ResourceWasBorned?.Invoke(resource);
    }

    private IEnumerator RepeatGetResource(float delay)
    {
        var wait = new WaitForSeconds(delay);

        while (enabled)
        {
            yield return wait;

            CreateResource();
        }
    }
}
