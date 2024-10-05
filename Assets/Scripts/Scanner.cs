using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _resourceSpawner;

    protected Queue<Resource> ResourceActive = new();

    private void OnEnable()
    {
        _resourceSpawner.ResourceWasBorned += FindResource;
    }

    private void OnDisable()
    {
        _resourceSpawner.ResourceWasBorned -= FindResource;
    }

    private void FindResource(Resource resource)
    {
        ResourceActive.Enqueue(resource);
    }
}
