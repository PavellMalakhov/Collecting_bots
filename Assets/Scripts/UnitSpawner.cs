using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSpawner : Spawner<Unit>
{
    private List<Vector3> _parkingSpace = new();

    protected override void Awake()
    {
        base.Awake();

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

    protected override void Init(Unit unit)
    {
        int unitPositionNumber = Random.Range(0, _parkingSpace.Count());

        Vector3 unitPosition = transform.position + _parkingSpace[unitPositionNumber];

        unit.transform.SetPositionAndRotation(unitPosition, Quaternion.LookRotation(transform.position + unitPosition));

        _parkingSpace.RemoveAt(unitPositionNumber);
    }
}
