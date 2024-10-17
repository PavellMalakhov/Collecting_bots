using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    private readonly string MouseX = "Mouse X";
    private readonly string MouseY = "Mouse Y";

    [SerializeField] private bool _isClickSetFlag = false;

    public Vector3 MoveDirection { get; private set; }
    public Vector2 LookDirection { get; private set; }

    private void Update()
    {
        MoveDirection = new Vector3(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical));

        LookDirection = new Vector2(Input.GetAxis(MouseX), - Input.GetAxis(MouseY));

        if (Input.GetMouseButtonDown(0))
        {
            _isClickSetFlag = true;
        }
    }

    public bool GetClickSetFlag() => GetBoolAsTrigger(ref _isClickSetFlag);

    private bool GetBoolAsTrigger(ref bool value)
    {
        bool localValue = value;
        value = false;
        return localValue;
    }
}
