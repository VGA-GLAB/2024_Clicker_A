using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class IncreaseFacilities : MonoBehaviour
{
    ResourceManager _resourceManager;
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _product;
    [SerializeField] string _productName;
    [SerializeField] TextMeshProUGUI _productText;
    private void Start()
    {
        _resourceManager = FindAnyObjectByType<ResourceManager>();
        ResourceManager.Product p = _resourceManager.Products.Find(p => p.Name == _productName);
        _productText.text = $"{_productName}:{p.UnitCount}";
    }


    /// <summary>
    /// é{ê›ÇÃUIÇëùÇ‚Ç∑
    /// </summary>
    public void IncreaseProductUI(string name)
    {
        Debug.Log(name);
        ResourceManager.Product p = _resourceManager.Products.Find(p => p.Name == name);
        Debug.Log(p.CanBuy);
        if (p.CanBuy)
        {
            Debug.Log(p.UnitCount);
            _productText.text = $"{name}:{p.UnitCount + 1}";
            GameObject product = Instantiate(_product,_panel.transform);
        }
    }
}
