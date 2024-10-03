using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

public class Dispatcher : MonoBehaviour
{
    [SerializeField] private Unit _prefabUnit;
    [SerializeField] private ResourceSpawner _resourceSpawner;
 
    private Queue<Resource> _resourceActive = new();
    private Queue<Unit> _units = new();
    private List<Vector3> _parkingSpace = new List<Vector3> { 
        new Vector3(5f, 0, 0),
        new Vector3(0, 0, 5f),
        new Vector3(-5f, 0, 0),
        new Vector3(0, 0, -5f),
        new Vector3(3.54f, 0, 3.54f),
        new Vector3(3.54f, 0, -3.54f),
        new Vector3(-3.54f, 0, 3.54f),
        new Vector3(-3.54f, 0, -3.54f), };

    public event Action<Resource> ResourceUnloaded;

    private void Start()
    {
        GetUnit();
        GetUnit();
        GetUnit();
    }

    private void OnEnable()
    {
        _resourceSpawner.ResourceWasBorned += FindResource;
    }

    private void OnDisable()
    {
        _resourceSpawner.ResourceWasBorned -= FindResource;
    }

    private void FixedUpdate()
    {
        if (_units.Count > 0 && _resourceActive.Count > 0)
        {
            DeliverResource();
        }
    }

    private void DeliverResource()
    {
        Sequence _sequence = DOTween.Sequence();

        var unit = _units.Dequeue();
        var resource = _resourceActive.Dequeue();
        var parkingSpace = unit.transform.position;

        _sequence.Append(unit.transform.DOLookAt(resource.transform.position, 1f));
        _sequence.Append(unit.transform.DOMove(resource.transform.position - (resource.transform.position - parkingSpace).normalized * 3, 2f));
        _sequence.Append(unit.transform.DOLookAt(parkingSpace, 1f));

        StartCoroutine(PickUpResource(unit, resource));

        _sequence.Append(unit.transform.DOMove(parkingSpace, 2f));
        _sequence.Append(unit.transform.DOLookAt(transform.position + parkingSpace + parkingSpace, 1f));

        StartCoroutine(Unloading(unit, resource));
    }

    private IEnumerator PickUpResource(Unit unit, Resource resource)
    {
        var wait = new WaitForSeconds(4f);

        yield return wait;

        resource.transform.SetParent(unit.transform);
    }

    private IEnumerator Unloading(Unit unit, Resource resource)
    {
        var wait = new WaitForSeconds(7f);

        yield return wait;

        resource.transform.SetParent(null);
        ResourceUnloaded?.Invoke(resource);
        _units.Enqueue(unit);
    }

    private void GetUnit()
    {
        int unitPositionNumber = UnityEngine.Random.Range(0, _parkingSpace.Count());
        Vector3 unitPosition = transform.position + _parkingSpace[unitPositionNumber];

        _units.Enqueue(Instantiate(_prefabUnit, unitPosition, Quaternion.LookRotation(transform.position + unitPosition), transform));
        _parkingSpace.RemoveAt(unitPositionNumber);
    }

    private void FindResource(Resource resource)
    {
        _resourceActive.Enqueue(resource);
    }
}
