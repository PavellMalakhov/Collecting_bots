using UnityEngine;

public class Looking : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _player;

    public void LookRotate(Vector2 lookDirection)
    {
        _player.Rotate(_rotateSpeed * lookDirection.x * Vector3.up * Time.deltaTime);

        _camera.Rotate(_rotateSpeed * lookDirection.y * Vector3.right * Time.deltaTime);
    }
}
