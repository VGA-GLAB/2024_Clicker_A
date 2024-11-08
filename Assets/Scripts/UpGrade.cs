using System.Numerics;
using UnityEngine;

public class UpGrade : MonoBehaviour
{
    [SerializeField] string _upGradeProductName;
    [SerializeField] uint _upGradeRate;
    [SerializeField] string _price;
    private BigInteger _updatePrice;
    ResourceManager _resourceManager;
    void Start()
    {
        _resourceManager = GameObject.FindAnyObjectByType<ResourceManager>();
        _updatePrice = BigInteger.Parse(_price);
    }

    public void UpGradeProduct()
    {
        if (_resourceManager.Resource >= _updatePrice)
        {
            _resourceManager.UpGradeProduct(_upGradeProductName, _upGradeRate, _updatePrice);
            Destroy(this.gameObject);
        }
    }
    public void UpGradeProductAndClick()
    {
        if (_resourceManager.Resource >= _updatePrice)
        {
            _resourceManager.UpGradeProductAndClick(_upGradeProductName, _upGradeRate, _updatePrice);
            Destroy(this.gameObject);
        }
    }
}
