using System.Collections;
using UnityEngine;

public class ResourceSpawner : Spawner<Resource>
{
    [SerializeField] private float _repeatTime = 2f;
    [SerializeField] private float _resourceAreaSize = 50;
    [SerializeField] private float _heightArea = 0;

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

    protected internal override void Init(Resource resource)
    {
        resource.transform.position = new Vector3(
            UnityEngine.Random.Range(-_resourceAreaSize, _resourceAreaSize), _heightArea,
            UnityEngine.Random.Range(-_resourceAreaSize, _resourceAreaSize));

        resource.Recycled += ReleaseGameObject;
    }

    protected internal override void ReleaseGameObject(Resource resource)
    {
        resource.Recycled -= ReleaseGameObject;

        base.ReleaseGameObject(resource);
    }
}
