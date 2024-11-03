using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private BigInteger _resource = 0;
    public BigInteger Resource { get => _resource; set => _resource = value; }
    private BigInteger _increaseAmount = 0;
    private BigInteger _increaseAmountOnClick = 1;
    [SerializeField] List<Product> _products;
    [Serializable]
    public struct Product
    {
        public string Name;
        public string ProductionSpeedPerUnit;
        public string PricePerUnit;
        public int UnitCount;
        public BigInteger ProductionPerSecond;
        public BigInteger DefaultPrice;
        public BigInteger Price;
        public float ProductionRate;
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _products.Count; i++)
        {
            Product p = _products[i];
            p.DefaultPrice = BigInteger.Parse(_products[i].PricePerUnit);
            _products[i] = p;
        }
        StartCoroutine(GainPerSecond());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GainPerSecond()
    {
        IncreaseResource(_increaseAmount);
        yield return new WaitForSeconds(1);
    }
    void IncreaseResource(BigInteger resource)
    {
        _resource += resource;
    }
    public void IncreaseResourceOnClick()
    {
        IncreaseResource(_increaseAmountOnClick);
    }
    public void BuyProduct(string name)
    {
        Product p = _products.Find(p => p.Name == name);
        p.UnitCount++;
        p.Price = p.DefaultPrice * BigInteger.Pow(115,p.UnitCount)/BigInteger.Pow(100,p.UnitCount);
        //p.ProductionPerSecond = p.Price * BigInteger.Parse(p.ProductionRate);
    }

}
