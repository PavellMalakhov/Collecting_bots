using TMPro;
using UnityEngine;
using System;

public class ResourceTextDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentResourceValue;
    [SerializeField] private Inventory _inventory;

    private void OnEnable()
    {
        _inventory.ResourceChanged += RenderResourceValue;
    }

    private void OnDisable()
    {
        _inventory.ResourceChanged -= RenderResourceValue;
    }

    public void RenderResourceValue(int resourceValue)
    {
        _currentResourceValue.text = Convert.ToString(resourceValue);
    }
}
