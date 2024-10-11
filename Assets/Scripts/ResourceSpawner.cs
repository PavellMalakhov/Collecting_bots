using System.Collections;
using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [SerializeField] private float _repeatTime = 2f;
    [SerializeField] private float _resourceAreaSize = 50;
    [SerializeField] private float _heightArea = 0;
    [SerializeField] private Base _base;

    private void OnEnable()
    {
        _base.ResourceUnloaded += ReleaseGameObject;
    }

    private void OnDisable()
    {
        _base.ResourceUnloaded -= ReleaseGameObject;
    }

    private void Start()
    {
        StartCoroutine(RepeatGetResource(_repeatTime));
    }

    private IEnumerator RepeatGetResource(float delay)
    {
        var wait = new WaitForSeconds(delay);

        while (enabled)
        {
            yield return wait;

            CreateGameObject();
        }
    }

    protected override void Init(Resource resource)
    {
        resource.transform.position = new Vector3(
            UnityEngine.Random.Range(-_resourceAreaSize, _resourceAreaSize), _heightArea,
            UnityEngine.Random.Range(-_resourceAreaSize, _resourceAreaSize));
    }
}
