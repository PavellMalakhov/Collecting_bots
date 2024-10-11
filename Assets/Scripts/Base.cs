using System.Collections.Generic;
using UnityEngine;
using System;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private int _unitStartValue;
 
    private Queue<Resource> _resourceForDelivery = new ();
    private Queue<Unit> _unitsForDelivery = new();

    public event Action<Resource> ResourceUnloaded;

    private void OnEnable()
    {
        _scanner.ResourceFounded += AddResourseForDelivery;
    }

    private void OnDisable()
    {
        _scanner.ResourceFounded -= AddResourseForDelivery;
    }

    private void Start()
    {
        CreateUnits(_unitStartValue);
    }

    private void FixedUpdate()
    {
        if (_unitsForDelivery.Count > 0 && _resourceForDelivery.Count > 0)
        {
            _unitsForDelivery.Dequeue().DeliverResource(_resourceForDelivery.Dequeue());
        }
    }

    private void AddResourseForDelivery(Resource resource)
    {
        _resourceForDelivery.Enqueue(resource);
    }

    private void CreateUnits(int unitStartValue)
    {
        for (int i = 0; i < unitStartValue; i++)
        {
            Unit unit = _unitSpawner.CreateGameObject();

            unit.transform.SetParent(transform);

            unit.Unloaded += EnqueueUnit;

            unit.Disable += Unsubscribe;

            _unitsForDelivery.Enqueue(unit);
        }
    }

    private void EnqueueUnit(Unit unit, Resource resource)
    {
        ResourceUnloaded?.Invoke(resource);

        _unitsForDelivery.Enqueue(unit);
    }

    private void Unsubscribe(Unit unit)
    {
        unit.Unloaded -= EnqueueUnit;

        unit.Disable -= Unsubscribe;
    }
}
