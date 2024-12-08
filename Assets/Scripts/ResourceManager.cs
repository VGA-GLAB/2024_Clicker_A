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
        [Tooltip("施設の個数を表示するテキスト")] public TextMeshProUGUI ProductCountText;
    }
    void Start()
    {
        Load();
        StartCoroutine(AutoSave());
    }
    IEnumerator AutoSave()
    {
        while (true)
        {
            Save();
            Debug.Log("セーブしました");
            yield return new WaitForSeconds(10);
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
        UpdateCanBuy();
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
            UpdateCanBuy();
            if (p.UnitCount == 1)
            {
                if(p.Name != "Cursor")
                StartCoroutine(GainPerSecond(p , 1));
                else
                StartCoroutine(GainPerSecond(p, 10));
            }
            //価格とリソース量の更新
            p.PriceText.text = $"{p.Name}:{p.Price}";
            _resourceText.text = _resource.ToString();
            p.ProductCountText.text = $"{p.Name}:{p.UnitCount}";
        }
    }
    private void UpdateCanBuy()
    {
        for (int i = 0; i < _products.Count; i++)
        {
            Product product = _products[i];
            product.CanBuy = product.Price <= _resource;
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
            UpdateCanBuy();
            p.ProductionRate *= rate;
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;

            // Fix: UpGrade購入時にリソース表示が更新されていない不具合を修正。
            _resourceText.text = _resource.ToString();
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
            UpdateCanBuy();
            p.ProductionRate *= rate;
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;
            _increaseAmountOnClick *= rate;

            // Fix: UpGrade購入時にリソース表示が更新されていない不具合を修正。
            _resourceText.text = _resource.ToString();
        }
    }
    public void Save()
    {
        PlayerPrefs.SetString("Resource",_resource.ToString());
        PlayerPrefs.SetString("IncreaseAmountOnClick", _increaseAmountOnClick.ToString());
        for (int i = 0; i < _products.Count; i++)
        {
            Product p = _products[i];
            PlayerPrefs.SetInt($"{p.Name}UnitCount",p.UnitCount);
            PlayerPrefs.SetString($"{p.Name}ProductionRate",p.ProductionRate.ToString());
        }
        PlayerPrefs.Save();
    }
    public void Load()
    {
        _resource = BigInteger.Parse(PlayerPrefs.GetString("Resource","0"));
        _increaseAmountOnClick = BigInteger.Parse(PlayerPrefs.GetString("IncreaseAmountOnClick","1"));
        for (int i = 0; i < _products.Count; i++)
        {
            Product p = _products[i];
            p.UnitCount = PlayerPrefs.GetInt($"{p.Name}UnitCount",0);
            p.DefaultPrice = BigInteger.Parse(p.PricePerUnit);
            p.ProductionRate = ulong.Parse(PlayerPrefs.GetString($"{p.Name}ProductionRate","1"));
            p.ProductionPerSecond = BigInteger.Parse(p.ProductionSpeedPerUnit);
            //価格を上げる
            p.Price = p.DefaultPrice * BigInteger.Pow(115, p.UnitCount) / BigInteger.Pow(100, p.UnitCount);
            //生産速度を更新
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;
            if (p.UnitCount >= 1)
            {
                if (p.Name != "Cursor")
                    StartCoroutine(GainPerSecond(p, 1));
                else
                    StartCoroutine(GainPerSecond(p, 10));
            }
            //価格とリソース量の更新
            p.PriceText.text = $"{p.Name}:{p.Price}";
            _resourceText.text = _resource.ToString();
            p.ProductCountText.text = $"{p.Name}:{p.UnitCount}";
        }
        UpdateCanBuy();
    }
}
