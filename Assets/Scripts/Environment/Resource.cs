using UnityEngine;
using System;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Recycled;
    public Scanner Scanner { get; private set; }

    public void SetScanner(Scanner scanner)
    {
        if (Scanner == null)
        {
            Scanner = scanner;
        }
    }

    public void Recycle()
    {
        Recycled?.Invoke(this);
    }

    private void OnEnable()
    {
        Scanner = null;
    }
}