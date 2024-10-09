using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Unit _prefabUnit;
    [SerializeField] private int _unitStartValue;

    private Queue<Resource> _resourceForDelivery = new ();
    private Queue<Unit> _units = new();
    private List<Vector3> _parkingSpace = new();

    public event Action<Unit, Resource> Deliver;
    public event Action<Resource> ResourceUnloaded;

    private void Awake()
    {
        _parkingSpace = new List<Vector3> {
        new Vector3(5f, 0, 0),
        new Vector3(0, 0, 5f),
        new Vector3(-5f, 0, 0),
        new Vector3(0, 0, -5f),
        new Vector3(3.54f, 0, 3.54f),
        new Vector3(3.54f, 0, -3.54f),
        new Vector3(-3.54f, 0, 3.54f),
        new Vector3(-3.54f, 0, -3.54f)};
    }

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
        for (int i = 0; i < _unitStartValue; i++)
        {
            CreateUnit();
        }
    }

    private void FixedUpdate()
    {
        if (_units.Count > 0 && _resourceForDelivery.Count > 0)
        {
            Deliver?.Invoke(_units.Dequeue(), _resourceForDelivery.Dequeue());
        }
    }

    private void AddResourseForDelivery(Resource resource)
    {
        _resourceForDelivery.Enqueue(resource);
    }

    private void CreateUnit()
    {
        int unitPositionNumber = UnityEngine.Random.Range(0, _parkingSpace.Count());
        Vector3 unitPosition = transform.position + _parkingSpace[unitPositionNumber];
        Unit unit = Instantiate(_prefabUnit, unitPosition, Quaternion.LookRotation(transform.position + unitPosition), transform);

        _units.Enqueue(unit);
        unit.Unloaded += EnqueueUnit;
        unit.Disable += Unsubscribe;
        _parkingSpace.RemoveAt(unitPositionNumber);
    }

    private void EnqueueUnit(Unit unit, Resource resource)
    {
        ResourceUnloaded?.Invoke(resource);

        _units.Enqueue(unit);
    }

    private void Unsubscribe(Unit unit)
    {
        unit.Unloaded -= EnqueueUnit;

        unit.Disable -= Unsubscribe;
    }
}
