using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MiniGameQuestsManager : MonoBehaviour
{
    [SerializeField] private List<BossParameter> _bossParameters;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _clickPowerText;
    [SerializeField] private Image _hpGauge;
    [SerializeField] private Canvas _canvas;
    /// <summary>
    /// ダメージポップアップ
    /// </summary>
    [SerializeField] private GameObject _damegeText;
    /// <summary>
    /// ダメージテキストの表示時間
    /// </summary>
    [SerializeField] private int _textLifeTime;
    private float _currentHP;
    [SerializeField] private float _timeLimit;
    /// <summary>
    /// クリックで与えた総ダメージ
    /// </summary>
    private int _damageTotal;
    [Header("1クリックあたりの与ダメージ量"), SerializeField] private int _clickDamage;
    private int _clickLevel;
    /// <summary>
    /// １秒あたりに与える自動ダメージ
    /// </summary>
    private int _autoDamage;
    /// <summary>
    /// 現在のボスのID
    /// </summary>
    private int _currentBossId;
    /// <summary>
    /// リザルトを表示するパネル
    /// </summary>
    [SerializeField] GameObject _resultPanel;
    
    [CreateAssetMenu(menuName = "ScriptableObject/BossParameter")]
    public class BossParameter : ScriptableObject
    {
        [Header("ボスのID")] public int Id;
        [Header("ボスの最大HP")] public int BossMaxHP;
        [Header("ボスの見た目")] public Sprite BossImage;
    }
    private void Start()
    {
        _resultPanel.SetActive(false);
        _currentBossId = Random.Range(0, 4);
        _currentHP = _bossParameters[_currentBossId].BossMaxHP;
        _clickPowerText.text = _clickDamage.ToString();
    }

    private void Update()
    {
        if (_timeLimit > 0)
        {
            Timer();
        }
        else
        {
            _resultPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(_currentBossId);
            Debug.Log(_currentHP);
        }//デバック用
    }

    /// <summary>
    /// 敵をクリックしたときダメージを与える処理
    /// </summary>
    public void ClickDamage()
    {
        _currentHP -= _clickDamage;
        _damageTotal += _clickDamage;
        Vector2 mousePos = Input.mousePosition;
        Vector2 localPosition;
        BossHP();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.GetComponent<RectTransform>(),mousePos,
                _canvas.worldCamera,
                out localPosition
            );//mousePosをローカルポジションに直す
        TextMeshProUGUI text = Instantiate(_damegeText, _canvas.transform).GetComponent<TextMeshProUGUI>();//クリックした場所にテキストを表示
        text.transform.localPosition = localPosition;
        text.text = _clickDamage.ToString();
        StartCoroutine(DestroyTextAfterTime(text, _textLifeTime));//上で表示したテキストを_textLifeTime秒後に消す
    }

    private void Timer()
    {
        _timeLimit -= Time.deltaTime;
        _timerText.text = _timeLimit.ToString("0.00");
    }

    /// <summary>
    /// ボスのHP表示を更新する処理
    /// </summary>
    private void BossHP()
    {
        BossParameter b = _bossParameters.Find(b => b.Id == _currentBossId);
        _hpGauge.fillAmount = _currentHP / b.BossMaxHP;
    }

    /// <summary>
    /// クリックレベルアップ
    /// </summary>
    private void LevelUp()
    {
        _clickPowerText.text = _clickDamage.ToString();
    }

    /// <summary>
    /// 数秒後にテキストを消す処理
    /// </summary>
    private IEnumerator DestroyTextAfterTime(TextMeshProUGUI textObject, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(textObject.gameObject);  // テキストを削除
    }
}
