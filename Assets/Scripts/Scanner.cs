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

    public event Action<Resource> ResourceFounded;

    private void Awake()
    {
        _wait = new WaitForSeconds(_scanRpeatTime);
    }

    private void Start()
    {
        StartCoroutine(ScanRepeater());
    }

    private IEnumerator ScanRepeater()
    {
        while (enabled)
        {
            yield return _wait;

            FindResources();
        }
    }

    private void FindResources()
    {
        ScanZone();

        AddNewActiveResource();

        RemoveInactiveResource();
    }

    private void ScanZone()
    {
        _resourceColliderInZone = Physics.OverlapBox(Vector3.zero, new Vector3(_resourceAreaSize, 0f, _resourceAreaSize),
            Quaternion.identity, _resourceLayerMask);

        foreach (var item in _resourceColliderInZone)
        {
            if (item.TryGetComponent<Resource>(out Resource resource))
            {
                _resourceInZone.Add(resource);
            }
        }
    }

    private void AddNewActiveResource()
    {
        foreach (var item in _resourceInZone)
        {
            if (!_resourceActive.Contains(item))
            {
                _resourceActive.Add(item);

                ResourceFounded?.Invoke(item);
            }
        }
    }

    private void RemoveInactiveResource()
    {
        for (int i = 0; i < _resourceActive.Count; i++)
        {
            if (!_resourceInZone.Contains(_resourceActive[i]))
            {
                _resourceActive.Remove(_resourceActive[i]);
            }
        }

        _resourceInZone.Clear();
    }
}
