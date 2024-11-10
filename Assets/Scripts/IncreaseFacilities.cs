using UnityEngine;

public class IncreaseFacilities : MonoBehaviour
{
    ResourceManager _resourceManager;
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _product;
    private void Start()
    {
        _resourceManager = FindAnyObjectByType<ResourceManager>();
    }

    /// <summary>
    /// {İ‚ÌUI‚ğ‘‚â‚·
    /// </summary>
    public void IncreaseProductUI(string name)
    {
        Debug.Log(name);
        ResourceManager.Product p = _resourceManager.Products.Find(p => p.Name == name);
        Debug.Log(p.CanBuy);
        if (p.CanBuy)
        {
            Debug.Log("{İ‚ğ’Ç‰Á");
            GameObject product = Instantiate(_product,_panel.transform);
        }
    }
}
