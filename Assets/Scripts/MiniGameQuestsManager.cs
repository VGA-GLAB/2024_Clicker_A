using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MiniGameQuestsManager : MonoBehaviour
{
    [SerializeField] private List<BossParameter> _bossParameters;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _hpGauge;
    private int _currentHP;
    private float _timeLimit;
    /// <summary>
    /// クリックで与えたそうダメージ
    /// </summary>
    private int _damageTotal;
    [Header("1クリックあたりの与ダメージ量"),SerializeField] private int _clickDamage;
    private int _clickLevel;
    /// <summary>
    /// １秒あたりに与える自動ダメージ
    /// </summary>
    private int _autoDamage;
    /// <summary>
    /// 現在のボスのID
    /// </summary>
    private int _currentBossId;

    public class BossParameter : ScriptableObject
    {
        [Header("ボスのID")] public int Id;
        [Header("ボスの最大HP")] public int BossMaxHP;
        [Header("ボスの見た目")] public Sprite BossImage;
    }
    private void Start()
    {
        _currentBossId = Random.Range(0, 4);
        _currentHP = _bossParameters[_currentBossId].BossMaxHP;
    }

    private void Update()
    {
        if (_timeLimit > 0)
        {
            Timer();
        }
    }

    private void Timer()
    {
        _timeLimit -= Time.deltaTime;
        _timerText.text = _timeLimit.ToString("0.00");
    }

    private void BossHP(int n)
    {
        BossParameter b = _bossParameters.Find(b => b.Id == _currentBossId);
        _hpGauge.fillAmount = _currentHP / b.BossMaxHP;
    }
}
