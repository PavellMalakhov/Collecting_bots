using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;

    private float _timeRotate = 1f;
    private float _timeTravel = 2f;

    public event Action<Unit, Resource> Unloaded;

    private void OnEnable()
    {
        _base = GetComponentInParent<Base>();

        _base.Deliver += DeliverResource;
    }

    private void OnDisable()
    {
        _base.Deliver -= DeliverResource;
    }

    private void DeliverResource(Unit unit, Resource resource)
    {
        if (unit == this)
        {
            Sequence _sequence = DOTween.Sequence();

            var parkingSpace = transform.position;
            float unitRatio = 3f;
            var placeToStop = resource.transform.position - (resource.transform.position - parkingSpace).normalized * unitRatio;

            _sequence.Append(transform.DOLookAt(resource.transform.position, _timeRotate));
            _sequence.Append(transform.DOMove(placeToStop, _timeTravel));
            _sequence.Append(transform.DOLookAt(parkingSpace, _timeRotate));
            _sequence.Append(transform.DOMove(parkingSpace, _timeTravel));
            _sequence.Append(transform.DOLookAt(transform.position + parkingSpace, _timeRotate));

            StartCoroutine(LoadUnloadResource(this, resource));
        }
    }

    private IEnumerator LoadUnloadResource(Unit unit, Resource resource)
    {
        var waitTravelTime = new WaitForSeconds(_timeRotate + _timeTravel + _timeRotate);
        var waitReturn = new WaitForSeconds(_timeTravel + _timeRotate);

        yield return waitTravelTime;

        resource.transform.SetParent(transform);

        yield return waitReturn;

        resource.transform.SetParent(null);
        Unloaded?.Invoke(this, resource);
    }
}
