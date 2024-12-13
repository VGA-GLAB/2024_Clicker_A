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
    private BigInteger _allResource = 0;
    public BigInteger Resource { get => _resource; set => _resource = value; }
    public List<Product> Products { get => _products; set => _products = value; }
    private BigInteger _cookiePerSecond = 0;
    private BigInteger _increaseAmountOnClick = 1;
    private int _clickBuff = 1;
    private int _productBuff = 100;
    [SerializeField] TextMeshProUGUI _resourceText;
    [SerializeField] TextMeshProUGUI _allResourceText;
    [SerializeField] TextMeshProUGUI _productionEfficiencyText;
    [SerializeField] TextMeshProUGUI _cookiePerSecondText;
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
        [Tooltip("施設全体の生産速度を表示するテキスト")] public TextMeshProUGUI ResourcePerSecondText;
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
            yield return new WaitForSeconds(60);
        }
    }
    public void GoldenCookie()
    {
        int n = UnityEngine.Random.Range(0, 100);
        if (n < 5)
        {
            StartCoroutine(BuffTime(777, 20));
        }//5%の確率で20秒魔力生産量が777倍
        else if (n < 20)
        {
            StartCoroutine(ClickBuffTime(100, 30));
        }//15%の確率で30秒クリックの生産量が100倍
        else if (n < 50)
        {
            IncreaseResource(_resource * 2 / 10);
        }//30%の確率で所持魔力の20%を即座に獲得
        else
        {
            StartCoroutine(BuffTime(7, 60));
        }//50%の確率で1分魔力生産量が7倍

    }
    IEnumerator ClickBuffTime(int rate, int time)
    {
        _clickBuff *= rate;
        yield return new WaitForSeconds(time);
        _clickBuff /= rate;
    }
    IEnumerator BuffTime(int rate, int time)
    {
        _productBuff *= rate;
        _productionEfficiencyText.text = _productBuff.ToString();
        _cookiePerSecond *= _productBuff / 100;
        _cookiePerSecondText.text = _cookiePerSecond.ToString();
        yield return new WaitForSeconds(time);
        _cookiePerSecond /= _productBuff / 100;
        _cookiePerSecondText.text = _cookiePerSecond.ToString();
        _productBuff /= rate;
        _productionEfficiencyText.text = _productBuff.ToString();
    }
    /// <summary>
    /// 毎秒リソースを増やす
    /// </summary>
    /// <returns></returns>
    IEnumerator GainPerSecond(Product p, int interval)
    {
        while (true)
        {
            IncreaseResource(p.ResourcePerSecond * _productBuff / 100);
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
        _allResource += resource;
        _allResourceText.text = _allResource.ToString();
        _resourceText.text = _resource.ToString();
        UpdateCanBuy();
    }
    /// <summary>
    /// クリック時にリソースを増やす
    /// </summary>
    public void IncreaseResourceOnClick()
    {
        IncreaseResource(_increaseAmountOnClick * _clickBuff * _productBuff / 100);
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
                if (p.Name != "Cursor")
                    StartCoroutine(GainPerSecond(p, 1));
                else
                    StartCoroutine(GainPerSecond(p, 10));
            }
            //価格とリソース量の更新
            p.PriceText.text = p.Price.ToString();
            _resourceText.text = _resource.ToString();
            p.ProductCountText.text = p.UnitCount.ToString();
            p.ResourcePerSecondText.text = p.ResourcePerSecond.ToString();
            _cookiePerSecond += p.ProductionPerSecond * p.ProductionRate * _productBuff / 100 / (p.Name == "Cursor" ? 10 : 1);
            _cookiePerSecondText.text = _cookiePerSecond.ToString();
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
        if (price <= _resource)
        {
            _resource -= price;
            UpdateCanBuy();
            p.ProductionRate *= rate;
            _cookiePerSecond -= p.ResourcePerSecond * _productBuff / (p.Name == "Cursor" ? 10 : 1);
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;
            _cookiePerSecond += p.ProductionPerSecond * _productBuff / (p.Name == "Cursor" ? 10 : 1);
            _cookiePerSecondText.text = _cookiePerSecond.ToString();
            p.ResourcePerSecondText.text = p.ResourcePerSecond.ToString();
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
            _cookiePerSecond -= p.ResourcePerSecond * _productBuff / (p.Name == "Cursor" ? 10 : 1);
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;
            _cookiePerSecond += p.ProductionPerSecond * _productBuff / (p.Name == "Cursor" ? 10 : 1);
            _cookiePerSecondText.text = _cookiePerSecond.ToString();
            p.ResourcePerSecondText.text = p.ResourcePerSecond.ToString();
            _increaseAmountOnClick *= rate;

            // Fix: UpGrade購入時にリソース表示が更新されていない不具合を修正。
            _resourceText.text = _resource.ToString();
        }
    }
    public void Save()
    {
        PlayerPrefs.SetString("Resource", _resource.ToString());
        PlayerPrefs.SetString("AllResource", _allResource.ToString());
        PlayerPrefs.SetString("IncreaseAmountOnClick", _increaseAmountOnClick.ToString());
        PlayerPrefs.SetString("CookiePerSecond", (_cookiePerSecond / (_productBuff / 100)).ToString());
        for (int i = 0; i < _products.Count; i++)
        {
            Product p = _products[i];
            PlayerPrefs.SetInt($"{p.Name}UnitCount", p.UnitCount);
            PlayerPrefs.SetString($"{p.Name}ProductionRate", p.ProductionRate.ToString());
        }
        PlayerPrefs.Save();
        Debug.Log("セーブしました");
    }
    public void Load()
    {
        StopAllCoroutines();
        //バフの初期化
        _clickBuff = 1;
        _productBuff = 100;
        //セーブデータのロード
        _resource = BigInteger.Parse(PlayerPrefs.GetString("Resource", "0"));
        _allResource = BigInteger.Parse(PlayerPrefs.GetString("AllResource", "0"));
        _allResourceText.text = _allResource.ToString();
        _increaseAmountOnClick = BigInteger.Parse(PlayerPrefs.GetString("IncreaseAmountOnClick", "1"));
        _cookiePerSecond = BigInteger.Parse(PlayerPrefs.GetString("CookiePerSecond", "0"));
        _cookiePerSecondText.text = _cookiePerSecond.ToString();
        for (int i = 0; i < _products.Count; i++)
        {
            Product p = _products[i];
            p.UnitCount = PlayerPrefs.GetInt($"{p.Name}UnitCount", 0);
            p.DefaultPrice = BigInteger.Parse(p.PricePerUnit);
            p.ProductionRate = ulong.Parse(PlayerPrefs.GetString($"{p.Name}ProductionRate", "1"));
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
            p.PriceText.text = p.Price.ToString();
            _resourceText.text = _resource.ToString();
            p.ProductCountText.text = p.UnitCount.ToString();
            p.ResourcePerSecondText.text = p.ResourcePerSecond.ToString();
            _clickBuff = 1;
            _productBuff = 100;
            _productionEfficiencyText.text = _productBuff.ToString();
        }
        UpdateCanBuy();
    }
    public void ResetSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("AllResource", _allResource.ToString());
        Start();
        UpgradeManager upgradeManager = FindAnyObjectByType<UpgradeManager>();
        upgradeManager.ReStart();
    }
}
