using UnityEngine;
using System;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Recycled;

    public bool IsFound { get; private set; }

    public void Find ()
    {
        if (!IsFound)
        {
            IsFound = true;
        }
    }

    public void Recycle()
    {
        Recycled?.Invoke(this);
    }

    private void OnEnable()
    {
        IsFound = false;
    }
}