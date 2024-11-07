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
        [Tooltip("�{�ݖ�")] public string Name;
        [Tooltip("1�{�ݓ�����̐��Y���x")] public string ProductionSpeedPerUnit;
        [Tooltip("�{�݂̏������i")] public string PricePerUnit;
        [Tooltip("�{�݂̌�")] public int UnitCount;
        [Tooltip("1�{�ݓ�����̐��Y���x")] public BigInteger ProductionPerSecond;
        [Tooltip("�{�݂̏������i")] public BigInteger DefaultPrice;
        [Tooltip("�{�݂̉��i")] public BigInteger Price;
        [Tooltip("�{�݂̐��Y�{��")] public ulong ProductionRate;
        [Tooltip("�{�ݑS�̂̐��Y���x")] public BigInteger ResourcePerSecond;
        [Tooltip("�w���\���ǂ���")] public bool CanBuy;
        [Tooltip("���i��\������e�L�X�g")] public TextMeshProUGUI PriceText;
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
    /// ���b���\�[�X�𑝂₷
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
    /// ���\�[�X�𑝂₷
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
    /// �N���b�N���Ƀ��\�[�X�𑝂₷
    /// </summary>
    public void IncreaseResourceOnClick()
    {
        IncreaseResource(_increaseAmountOnClick);
    }
    /// <summary>
    /// �{�݂̍w��
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
            //���i���グ��
            p.Price = p.DefaultPrice * BigInteger.Pow(115, p.UnitCount) / BigInteger.Pow(100, p.UnitCount);
            //p.PriceText.text = p.Price.ToString();
            //���Y���x���X�V
            p.ResourcePerSecond = p.ProductionPerSecond * p.UnitCount * p.ProductionRate;
            _increaseAmount += p.ResourcePerSecond;
        }
    }
}
