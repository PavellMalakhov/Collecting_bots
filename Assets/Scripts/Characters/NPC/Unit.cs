using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    
    private float _speed = 10f;
    private float _timeRotate = 1f;
    private float _timeTravel;
    private WaitForSeconds _waitTravelTime;
    private WaitForSeconds _waitReturn;
    private WaitForFixedUpdate wait;
    private Vector3 _parkingSpace;

    public event Action<Unit, Resource> Unloaded;
    public event Action<Unit, Vector3> Disable;

    public void DeliverResource(Resource resource)
    {
        Sequence sequence = DOTween.Sequence();
        _parkingSpace = transform.position;
        Vector3 parkingDirection = transform.forward;
        float unitRatio = 3f;

        Vector3 placeToStop = resource.transform.position - (resource.transform.position - _parkingSpace).normalized * unitRatio;

        _timeTravel = (placeToStop - _parkingSpace).magnitude / _speed;

        _waitTravelTime = new WaitForSeconds(_timeRotate + _timeTravel + _timeRotate);
        _waitReturn = new WaitForSeconds(_timeTravel + _timeRotate);

        sequence.Append(transform.DOLookAt(resource.transform.position, _timeRotate));
        sequence.Append(transform.DOMove(placeToStop, _timeTravel));
        sequence.Append(transform.DOLookAt(_parkingSpace, _timeRotate));
        sequence.Append(transform.DOMove(_parkingSpace, _timeTravel));
        sequence.Append(transform.DOLookAt(_parkingSpace + parkingDirection, _timeRotate));

        StartCoroutine(LoadUnloadResource(resource));
    }

    private IEnumerator LoadUnloadResource(Resource resource)
    {
        yield return _waitTravelTime;

        resource.transform.SetParent(transform);

        yield return _waitReturn;

        resource.transform.SetParent(null);

        Unloaded?.Invoke(this, resource);

        resource.Recycle();
    }

    public void BuildNewBase(Flag flag)
    {
        Sequence sequence = DOTween.Sequence();

        _parkingSpace = transform.position;
        Vector3 placeToStop = flag.transform.position;

        _timeTravel = (placeToStop - _parkingSpace).magnitude / _speed;

        sequence.Append(transform.DOLookAt(placeToStop, _timeRotate));
        sequence.Append(transform.DOMove(placeToStop, _timeTravel));

        StartCoroutine (BuildBase(_timeTravel, placeToStop, flag));
    }

    private IEnumerator BuildBase(float timeTravel, Vector3 placeToStop, Flag flag)
    {
        float timeBuildBase = 1f;

        var wait = new WaitForSeconds(timeTravel + timeBuildBase);
        
        yield return wait;

        Instantiate(_basePrefab, placeToStop, Quaternion.identity);

        flag.Disable();

        Disable?.Invoke(this, _parkingSpace);
    }
}
