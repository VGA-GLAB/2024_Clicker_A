using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResourceManager : MonoBehaviour
{
    private BigInteger _resource = 0;
    public BigInteger Resource { get => _resource; set => _resource = value; }
    private BigInteger _increaseAmount = 0;
    private BigInteger _increaseAmountOnClick = 1;
    [SerializeField] TextMeshProUGUI _resourceText;
    [SerializeField] List<Product> _products;
    [Serializable]
    public class Product
    {
        [Tooltip("{İ–¼")] public string Name;
        [Tooltip("1{İ“–‚½‚è‚Ì¶Y‘¬“x")] public string ProductionSpeedPerUnit;
        [Tooltip("{İ‚Ì‰Šú‰¿Ši")] public string PricePerUnit;
        [Tooltip("{İ‚ÌŒÂ”")] public int UnitCount;
        [Tooltip("1{İ“–‚½‚è‚Ì¶Y‘¬“x")] public BigInteger ProductionPerSecond;
        [Tooltip("{İ‚Ì‰Šú‰¿Ši")] public BigInteger DefaultPrice;
        [Tooltip("{İ‚Ì‰¿Ši")] public BigInteger Price;
        [Tooltip("{İ‚Ì¶Y”{—¦")] public ulong ProductionRate;
        [Tooltip("{İ‘S‘Ì‚Ì¶Y‘¬“x")] public BigInteger ResourcePerSecond;
        [Tooltip("w“ü‰Â”\‚©‚Ç‚¤‚©")] public bool CanBuy;
        [Tooltip("‰¿Ši‚ğ•\¦‚·‚éƒeƒLƒXƒg")] public TextMeshProUGUI PriceText;
    }
    void Start()
    {
        for (int i = 0; i < _products.Count; i++)
        {
            Product p = _products[i];
            p.DefaultPrice = BigInteger.Parse(p.PricePerUnit);
            p.Price = p.DefaultPrice;
            p.ProductionPerSecond = BigInteger.Parse(p.ProductionSpeedPerUnit);
            //p.PriceText.text = p.PricePerUnit;
            p.ProductionRate = 1;
        }
        StartCoroutine(GainPerSecond());
    }
    void Update()
    {

    }
    /// <summary>
    /// –ˆ•bƒŠƒ\[ƒX‚ğ‘‚â‚·
    /// </summary>
    /// <returns></returns>
    IEnumerator GainPerSecond()
    {
        while (true)
        {
            IncreaseResource(_increaseAmount);   
            yield return new WaitForSeconds(1);
        }   
    }
    /// <summary>
    /// ƒŠƒ\[ƒX‚ğ‘‚â‚·
    /// </summary>
    /// <param name="resource"></param>
    void IncreaseResource(BigInteger resource)
    {
        _resource += resource;
        _resourceText.text = _resource.ToString();
        for (int i = 0; i < _products.Count; i++)
        {
            Product p = _products[i];
            p.CanBuy = p.Price <= _resource;
        }
    }
    /// <summary>
    /// ƒNƒŠƒbƒN‚ÉƒŠƒ\[ƒX‚ğ‘‚â‚·
    /// </summary>
    public void IncreaseResourceOnClick()
    {
        IncreaseResource(_increaseAmountOnClick);
    }
    /// <summary>
    /// {İ‚Ìw“ü
    /// </summary>
    /// <param name="name"></param>
    public void BuyProduct(string name)
    {
        Product p = _products.Find(p => p.Name == name);    
        if(p.CanBuy)
        {
            _increaseAmount -= p.ResourcePerSecond;
            _resource -= p.Price;
            p.UnitCount++;
            //‰¿Ši‚ğã‚°‚é
            p.Price = p.DefaultPrice * BigInteger.Pow(115, p.UnitCount) / BigInteger.Pow(100, p.UnitCount);
            //p.PriceText.text = p.Price.ToString();
            //¶Y‘¬“x‚ğXV
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;
            _increaseAmount += p.ResourcePerSecond;
        }
    }
}
