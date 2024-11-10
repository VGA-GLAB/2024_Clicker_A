using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

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
    /// é{ê›ÇÃUIÇëùÇ‚Ç∑
    /// </summary>
    public void IncreaseProductUI(string name)
    {
        Debug.Log(name);
        ResourceManager.Product p = _resourceManager.Products.Find(p =>  p.Name == name);
        Debug.Log(p.CanBuy);
        if (p.CanBuy)
        {
            Debug.Log("é{ê›Çí«â¡");
            GameObject product = Instantiate(_product);
            product.transform.SetParent(_panel.transform);
        }
    }

}
