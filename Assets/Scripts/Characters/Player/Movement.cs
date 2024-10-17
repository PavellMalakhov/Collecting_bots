using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    public void Move(Vector3 moveDirection)
    {
        transform.Translate(moveDirection * _moveSpeed * Time.deltaTime);
    }
}
