using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Dispatcher _dispatcher;
    [SerializeField] private int _resource = 0;
    [SerializeField] private ResourceTextDisplay _resourceTextDisplay;

    private void OnEnable()
    {
        _dispatcher.ResourceUnloaded += AddResource;
    }

    private void OnDisable()
    {
        _dispatcher.ResourceUnloaded -= AddResource;
    }

    private void AddResource(Resource resource)
    {
        _resource++;

        _resourceTextDisplay.RenderResource(_resource);
    }
}
