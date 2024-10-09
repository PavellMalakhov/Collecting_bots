using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;

    private float _timeRotate = 1f;
    private float _timeTravel = 2f;
    private WaitForSeconds _waitTravelTime;
    private WaitForSeconds _waitReturn;

    public event Action<Unit, Resource> Unloaded;
    public event Action<Unit> Disable;

    private void Awake()
    {
        _waitTravelTime = new WaitForSeconds(_timeRotate + _timeTravel + _timeRotate);
        _waitReturn = new WaitForSeconds(_timeTravel + _timeRotate);
    }

    private void OnEnable()
    {
        _base = GetComponentInParent<Base>();

        _base.Deliver += DeliverResource;
    }

    private void OnDisable()
    {
        _base.Deliver -= DeliverResource;

        Disable?.Invoke(this);
    }

    private void DeliverResource(Unit unit, Resource resource)
    {
        if (unit == this)
        {
            Sequence sequence = DOTween.Sequence();

            var parkingSpace = transform.position;
            float unitRatio = 3f;
            var placeToStop = resource.transform.position - (resource.transform.position - parkingSpace).normalized * unitRatio;

            sequence.Append(transform.DOLookAt(resource.transform.position, _timeRotate));
            sequence.Append(transform.DOMove(placeToStop, _timeTravel));
            sequence.Append(transform.DOLookAt(parkingSpace, _timeRotate));
            sequence.Append(transform.DOMove(parkingSpace, _timeTravel));
            sequence.Append(transform.DOLookAt(transform.position + parkingSpace, _timeRotate));

            StartCoroutine(LoadUnloadResource(resource));
        }
    }

    private IEnumerator LoadUnloadResource(Resource resource)
    {
        yield return _waitTravelTime;

        resource.transform.SetParent(transform);

        yield return _waitReturn;

        resource.transform.SetParent(null);
        Unloaded?.Invoke(this, resource);
    }
}
