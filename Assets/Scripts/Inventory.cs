using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private int _resourceValue = 0;

    public event Action<int> ResourceChanged;

    private void OnEnable()
    {
        _base.ResourceUnloaded += AddResource;
    }

    private void OnDisable()
    {
        _base.ResourceUnloaded -= AddResource;
    }

    private void AddResource(Resource resource)
    {
        _resourceValue++;

        ResourceChanged?.Invoke(_resourceValue);
    }
}
