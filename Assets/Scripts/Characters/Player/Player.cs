using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Movement _movement;
    [SerializeField] private Looking _looking;
    [SerializeField] private FlagSeter _flagSeter;

    public event Action Building;

    private void FixedUpdate()
    {
        if (_inputReader.GetClickSetFlag())
        {
            Building?.Invoke();
        }
    }

    private void Update()
    {
        if (_inputReader.MoveDirection != Vector3.zero)
        {
            _movement.Move(_inputReader.MoveDirection);
        }

        if (_inputReader.LookDirection != Vector2.zero)
        {
            _looking.LookRotate(_inputReader.LookDirection);
        }
    }
}
