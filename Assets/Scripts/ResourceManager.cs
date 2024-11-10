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
    public List<Product> Products { get => _products; set => _products = value; }
    private BigInteger _increaseAmountOnClick = 1;
    [SerializeField] TextMeshProUGUI _resourceText;
    [SerializeField] List<Product> _products;
    [Serializable]
    public class Product
    {
        [Tooltip("施設名")] public string Name;
        [Tooltip("1施設当たりの生産速度")] public string ProductionSpeedPerUnit;
        [Tooltip("施設の初期価格")] public string PricePerUnit;
        [Tooltip("施設の個数")] public int UnitCount;
        [Tooltip("1施設当たりの生産速度")] public BigInteger ProductionPerSecond;
        [Tooltip("施設の初期価格")] public BigInteger DefaultPrice;
        [Tooltip("施設の価格")] public BigInteger Price;
        [Tooltip("施設の生産倍率")] public ulong ProductionRate;
        [Tooltip("施設全体の生産速度")] public BigInteger ResourcePerSecond;
        [Tooltip("購入可能かどうか")] public bool CanBuy;
        [Tooltip("価格を表示するテキスト")] public TextMeshProUGUI PriceText;
    }
    //小数点以下も扱える構造体の開発中
    [Serializable]
    public struct BigNumber
    {
        public int 何桁区切りか;
        public List<単位> NumberList;
        public void IncreaseNumber(BigNumber number)
        {
            for (int i = 0; i < NumberList.Count; i++)
            {
                NumberList[i].Number += number.NumberList[i].Number;
            }
            繰り上げ();
        }
        void 繰り上げ()
        {
            for (int i = 0; i < NumberList.Count - 1; i++)
            {
                NumberList[i + 1].Number += (int)(NumberList[i].Number / Mathf.Pow(10, 何桁区切りか - 1));
            }
        }
        void DecreaseNumber(BigNumber number)
        {
            for (int i = 0; i < NumberList.Count; i++)
            {
                NumberList[i].Number -= number.NumberList[i].Number;
            }
            繰り下げ();
        }
        void 繰り下げ()
        {
            for (int i = 0; i < NumberList.Count - 1; i++)
            {
                if(NumberList[i].Number < 0)
                {
                    NumberList[i].Number += (int)Mathf.Pow(10, 何桁区切りか);
                    NumberList[i + 1].Number -= 1;
                }
            }
        }
    }
    public class 単位
    {
        public string Name;
        public int Number;
    }
    void Start()
    {
        for (int i = 0; i < _products.Count; i++)
        {
            Product p = _products[i];
            p.DefaultPrice = BigInteger.Parse(p.PricePerUnit);
            p.Price = p.DefaultPrice;
            p.ProductionPerSecond = BigInteger.Parse(p.ProductionSpeedPerUnit);
            p.ProductionRate = 1;
            p.PriceText.text = $"{p.Name}:{p.PricePerUnit}";
        }
    }
    /// <summary>
    /// 毎秒リソースを増やす
    /// </summary>
    /// <returns></returns>
    IEnumerator GainPerSecond(Product p, int interval)
    {
        while (true)
        {
            IncreaseResource(p.ResourcePerSecond);
            yield return new WaitForSeconds(interval);
        }
    }
    /// <summary>
    /// リソースを増やす
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
    /// クリック時にリソースを増やす
    /// </summary>
    public void IncreaseResourceOnClick()
    {
        IncreaseResource(_increaseAmountOnClick);
    }
    /// <summary>
    /// 施設の購入
    /// </summary>
    /// <param name="name"></param>
    public void BuyProduct(string name)
    {
        Product p = _products.Find(p => p.Name == name);
        if (p.CanBuy)
        {
            _resource -= p.Price;
            p.UnitCount++;
            //価格を上げる
            p.Price = p.DefaultPrice * BigInteger.Pow(115, p.UnitCount) / BigInteger.Pow(100, p.UnitCount);
            //生産速度を更新
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;
            p.CanBuy = p.Price <= _resource;
            if (p.UnitCount == 1)
            {
                if(p.Name != "Cursor")
                StartCoroutine(GainPerSecond(p , 1));
                else
                StartCoroutine(GainPerSecond(p, 10));
            }
            p.PriceText.text = $"{p.Name}:{p.Price}";
        }
    }
    /// <summary>
    /// 施設のアップグレード
    /// </summary>
    /// <param name="name"></param>
    public void UpGradeProduct(string name, uint rate, BigInteger price)
    {
        Product p = _products.Find(p => p.Name == name);
        if(price <= _resource)
        {
            _resource -= price;
            p.ProductionRate *= rate;
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;
        }
    }
    /// <summary>
    /// クリックのアップグレード
    /// </summary>
    public void UpGradeProductAndClick(string name, uint rate, BigInteger price)
    {
        Product p = _products.Find(p => p.Name == name);
        if (price <= _resource)
        {
            _resource -= price;
            p.ProductionRate *= rate;
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;
            _increaseAmountOnClick *= rate;
        }
    }
}
