using System;
using UnityEngine;

public class FlagSeter : MonoBehaviour
{
    [SerializeField] private FlagPreviewer _flagPreview;
    [SerializeField] private Flag _flag;
    [SerializeField] private Transform _camera;
    [SerializeField] private Player _player;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _canDrawPreviewFlag;
    [SerializeField] private bool _canPutFlag;

    private RaycastHit _hitInfo;

    public event Action<Flag> FlagSet;

    private void Awake()
    {
        _camera = FindAnyObjectByType<Camera>().transform;
        _player = FindAnyObjectByType<Player>();
    }

    private void OnEnable()
    {
        _player.Building += Build;
    }

    private void OnDisable()
    {
        _player.Building -= Build;
    }

    private void FixedUpdate()
    {
        DrawPreviewFlag(_canDrawPreviewFlag);
    }

    private void OnMouseDown()
    {
        _canDrawPreviewFlag = !_canDrawPreviewFlag;
    }

    private void OnMouseUp()
    {
        _canPutFlag = !_canPutFlag;
    }

    private void DrawPreviewFlag(bool canDrawPrevieFlag)
    {
        if (Physics.Raycast(_camera.position, _camera.forward, out _hitInfo, float.MaxValue, _layerMask) && canDrawPrevieFlag)
        {
            if (!_flagPreview.IsActive)
            {
                _flagPreview.Enable();
            }

            _flagPreview.SetPosition(_hitInfo.point);
        }
        else
        {
            _flagPreview.Disable();
        }
    }
    
    public void Build()
    {
        if (_hitInfo.point != null && _canPutFlag && _canDrawPreviewFlag)
        {
            if (!_flag.IsActive)
            {
                _flag.Enable();
            }

            _flag.SetPosition(_hitInfo.point);

            _canDrawPreviewFlag = false;

            _canPutFlag = false;

            FlagSet?.Invoke(_flag);
        }
    }
}
