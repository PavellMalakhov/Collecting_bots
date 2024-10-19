using System.Collections.Generic;
using UnityEngine;
using System;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private int _unitStartValue;
    [SerializeField] private int _unitCreateValue;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private FlagSeter _flagSeter;
 
    private Queue<Resource> _resourceForDelivery = new ();
    private Queue<Unit> _unitsForDelivery = new();
    private bool _buildingNewBase = false;
    private int _resourceForCreatUnit = 3;
    private int _resourceForBuildNewBase = 5;
    private int _units = 0;
    private int _unitsMax = 8;
    private Flag _flag;

    public event Action<Resource> ResourceUnloaded;

    private void OnEnable()
    {
        _scanner.ResourceFounded += AddResourseForDelivery;
        _flagSeter.FlagSet += SetSavingUpResourcesForNewBase;
    }

    private void OnDisable()
    {
        _scanner.ResourceFounded -= AddResourseForDelivery;
        _flagSeter.FlagSet -= SetSavingUpResourcesForNewBase;
    }

    private void Start()
    {
        CreateUnits(_unitStartValue);
    }

    private void Work()
    {
        if (_buildingNewBase == true && _units > 1)
        {
            if (_unitsForDelivery.Count > 0 && _inventory.GetResource(_resourceForBuildNewBase) == _resourceForBuildNewBase)
            {
                _buildingNewBase = false;

                _unitsForDelivery.Dequeue().BuildNewBase(_flag);
            }
        }

        if (_unitsForDelivery.Count > 0 && _resourceForDelivery.Count > 0)
        {
            _unitsForDelivery.Dequeue().DeliverResource(_resourceForDelivery.Dequeue());
        }

        if (_buildingNewBase == false && _units < _unitsMax || _units == 1)
        {
            if (_inventory.GetResource(_resourceForCreatUnit) == _resourceForCreatUnit)
            {
                CreateUnits(_unitCreateValue);
            }
        }
    }

    private void SetSavingUpResourcesForNewBase(Flag flag)
    {
        _buildingNewBase = true;

        _flag = flag;

        Work();
    }

    private void AddResourseForDelivery(Resource resource)
    {
        _resourceForDelivery.Enqueue(resource);

        Work();
    }

    private void CreateUnits(int unitCreateValue)
    {
        for (int i = 0; i < unitCreateValue; i++)
        {
            Unit unit = _unitSpawner.CreateGameObject();

            unit.Unloaded += EnqueueUnit;

            unit.Disable += Unsubscribe;

            _unitsForDelivery.Enqueue(unit);

            _units++;

            Work();
        }
    }

    private void EnqueueUnit(Unit unit, Resource resource)
    {
        ResourceUnloaded?.Invoke(resource);

        _unitsForDelivery.Enqueue(unit);

        Work();
    }

    private void Unsubscribe(Unit unit, Vector3 parkingSpace)
    {
        unit.Unloaded -= EnqueueUnit;

        unit.Disable -= Unsubscribe;

        _unitSpawner.AddParkingSpace(parkingSpace);

        _unitSpawner.ReleaseGameObject(unit);

        _units--;

        Work();
    }
}
