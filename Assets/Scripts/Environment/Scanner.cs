using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scanner : MonoBehaviour
{
    [SerializeField] float _scanRpeatTime = 0.5f;
    [SerializeField] private float _resourceAreaSize = 50f;
    [SerializeField] private LayerMask _resourceLayerMask;

    private Collider[] _resourceColliderInZone;
    private List<Resource> _resourceInZone = new();
    private List<Resource> _resourceActive = new();
    private WaitForSeconds _wait;
    private float _variationScanRpeatTime = 0.05f;

    public event Action<Resource> ResourceFounded;

    private void Start()
    {
        StartCoroutine(ScanRepeater());
    }

    private IEnumerator ScanRepeater()
    {
        while (enabled)
        {
            _wait = new WaitForSeconds(UnityEngine.Random.Range(_scanRpeatTime + _variationScanRpeatTime, _scanRpeatTime - _variationScanRpeatTime));

            yield return _wait;

            FindResources();
        }
    }

    private void FindResources()
    {
        _resourceColliderInZone = Physics.OverlapBox(Vector3.zero, new Vector3(_resourceAreaSize, 0f, _resourceAreaSize),
            Quaternion.identity, _resourceLayerMask);

        foreach (var item in _resourceColliderInZone)
        {
            if (item.TryGetComponent<Resource>(out Resource resource) && !resource.IsFound)
            {
                resource.Find();
                
                ResourceFounded?.Invoke(resource);
            }
        }
    }
}
