using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scanner : MonoBehaviour
{
    private Resource[] _resourceInZone;
    private List<Resource> _resourceActive = new();
    private WaitForSeconds _wait;

    public event Action<Resource> ResourceFounded;

    protected virtual void Awake()
    {
        float scanRpeatTime = 0.5f;
        
        _wait = new WaitForSeconds(scanRpeatTime);
    }

    protected virtual void Start()
    {
        StartCoroutine(ScanRepeater());
    }

    private IEnumerator ScanRepeater()
    {
        while (enabled)
        {
            yield return _wait;

            Scan();
        }
    }

    private void Scan()
    {
        _resourceInZone = FindObjectsByType<Resource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        for (int i = 0; i < _resourceInZone.Length; i++)
        {
            if (_resourceActive.Contains(_resourceInZone[i]) == false)
            {
                _resourceActive.Add(_resourceInZone[i]);

                ResourceFounded?.Invoke(_resourceInZone[i]);
            }
        }

        RemoveInactiveResource();
    }

    private void RemoveInactiveResource()
    {
        int valueDiscrepancies = 0;

        for (int i = 0; i < _resourceActive.Count; i++)
        {
            for (int j = 0; j < _resourceInZone.Length; j++)
            {
                if (_resourceActive[i] != _resourceInZone[j])
                {
                    valueDiscrepancies++;
                }
            }

            if (valueDiscrepancies == _resourceInZone.Length)
            {
                _resourceActive.RemoveAt(i);
            }

            valueDiscrepancies = 0;
        }
    }
}
