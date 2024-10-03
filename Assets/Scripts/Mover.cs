using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;

    public void Move(Vector3 direction)
    {
        transform.Translate(direction * _moveSpeed * Time.deltaTime);
    }
}
