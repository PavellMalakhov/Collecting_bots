using TMPro;
using UnityEngine;
using System;

public class ResourceTextDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentResourceValue;

    public void RenderResource(int resource)
    {
        _currentResourceValue.text = Convert.ToString(resource);
    }
}
