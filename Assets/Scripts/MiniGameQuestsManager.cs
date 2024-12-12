using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DevilManager;
using Random = UnityEngine.Random;

public class MiniGameQuestsManager : MonoBehaviour
{
    [SerializeField] private List<BossParameter> _bossParameters;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _clickPowerText;
    [Header("ミニゲームで得た報酬を表示するテキスト"), SerializeField] private TextMeshProUGUI _rewardText;
    [Header("リザルトで表示するボスの最終HP"), SerializeField] private TextMeshProUGUI _resultHP;
    [Header("リザルトで表示する評価"), SerializeField] private TextMeshProUGUI _resultScore;
    [SerializeField] private Image _hpGauge;
    [SerializeField] private Canvas _canvas;
    [Header("スキルボタンをすべて入れる"), SerializeField] private Button[] _skillBottons;
    [Header("各ボタンのテキスト"), SerializeField] private TextMeshProUGUI[] _buttonTexts;
    [Header("発動したスキルを表示"), SerializeField] private TextMeshProUGUI _skillText;
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
    private float _damageTotal;
    [Header("1クリックあたりの与ダメージ量"), SerializeField] private float _clickDamage;
    private int _clickLevel;
    /// <summary>
    /// １秒あたりに与える自動ダメージ
    /// </summary>
    private float _autoDamage;
    /// <summary>
    /// 現在のボスのID
    /// </summary>
    private int _currentBossId;
    /// <summary>
    /// リザルトを表示するパネル
    /// </summary>
    [SerializeField] GameObject _resultPanel;

    DevilManager _devilManager;

    List<Devil> _devilList = new List<Devil>();

    private bool _isTimeStop = false;

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
        _clickPowerText.text = $"Power : {_clickDamage.ToString()}";
        _devilManager = FindAnyObjectByType<DevilManager>();
        StartCoroutine(AutoDamage());
        Debug.Log(_devilManager._devils[_devilManager._devils.Count - 1]._Name);

        for (int i = 0; i < _devilManager._devils.Count; i++)
        {
            Debug.Log(i);
            _devilList.Add(_devilManager._devils[i]);//ここでAddするリストは実際とは異なり使い魔編成のシステムが追加され次第変更します
            _autoDamage += _devilList[i]._autoClickPower;
            _clickDamage += _devilList[i]._clickPower;
        }
        StartCoroutine(ButtonEnabled());

        _skillText.text = "";
    }

    private void Update()
    {
        Timer();
        if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(_currentBossId);
            Debug.Log(_currentHP);
        }//デバック用

        if (_timeLimit <= 0)
        {
            for (int i = 0; i < _devilList.Count; i++)
            {
                _skillBottons[i].enabled = false;
                _skillBottons[i].image.color = new Color(0, 0, 0, 0);
                _skillBottons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }//タイマーが0になったらボタンを全て消す
    }

    private void SkillDamage(float damage)
    {
        _currentHP -= damage;
        _damageTotal += damage;
        BossHP();
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
                _canvas.GetComponent<RectTransform>(), mousePos,
                _canvas.worldCamera,
                out localPosition
            );//mousePosをローカルポジションに直す
        TextMeshProUGUI text = Instantiate(_damegeText, _canvas.transform).GetComponent<TextMeshProUGUI>();//クリックした場所にテキストを表示
        text.transform.localPosition = localPosition;
        text.text = _clickDamage.ToString();
    }

    private void Timer()
    {
        if (_timeLimit > 0)
        {
            _timeLimit -= Time.deltaTime;
            if (!_isTimeStop)
            {
                _timerText.text = _timeLimit.ToString("0.00");
            }
        }
        else
        {
            _skillText.text = "";
            _resultPanel.SetActive(true);
            _resultHP.text = $"BossHP : {_currentHP.ToString("00000000")}";
            _resultScore.text = $"Score : {ResultScore()}";
            _rewardText.text = $"Get : {0}";
        }//リザルトの表示
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
    /// リザルトに表示する評価を判定
    /// </summary>
    private string ResultScore()
    {
        string score = _currentHP switch
        {
            < -1000000 => "SSS",
            < -100000 => "SS",
            < -10000 => "S",
            < -1000 => "A",
            < -1 => "B",
            _ => "C"
        };
        return score;
    }

    /// <summary>
    /// スキルボタンに表示されたテキストからスキルを判定する
    /// </summary>
    public void Skill(TextMeshProUGUI text)
    {

        if (text.text == "Skill Failed")
        {
            _skillText.text = "Skill Failed";
            return;
        }

        Devil devil = _devilList.Find(devil => devil._Name == text.text);
        text.text = "";
        Debug.Log(devil._Name);
        switch (devil._skill)
        {
            case Skills.FlashDamage:
                SkillDamage(_clickDamage * (2 + devil._level));
                _skillText.text = $"{devil._Name}:FlashDamage";
                break; //瞬時ダメージ
            case Skills.AutoDamage:
                SkillDamage(_autoDamage * (5 + devil._level));
                _skillText.text = $"{devil._Name}:AutoDamage";
                break;
            case Skills.TimeDamage:
                SkillDamage(_timeLimit * (2 + devil._level));
                _skillText.text = $"{devil._Name}:TimeDamage";
                break;
            case Skills.FlashDmUp:
                _clickDamage += devil._clickPower * devil._level;
                _skillText.text = $"{devil._Name}:FlashDamageUP";
                break;
            case Skills.AutoDmUp:
                _autoDamage += devil._autoClickPower * devil._level;
                _skillText.text = $"{devil._Name}:AutoDamageUP";
                break;
            case Skills.StopTime:
                StartCoroutine(TimeStopper());
                _skillText.text = $"{devil._Name}:StopTime";
                break;
            case Skills.ChangeClick:
                _clickDamage += _autoDamage;
                _skillText.text = $"{devil._Name}:ChangeClickPower";
                _autoDamage = 0;
                break;
            case Skills.ChangeAutoClick:
                _autoDamage += _clickDamage;
                _clickDamage = 0;
                _skillText.text = $"{devil._Name}:ChangeAutoPower";
                break;
        }
    }

    IEnumerator AutoDamage()
    {
        while (_timeLimit >= 0)
        {
            yield return new WaitForSeconds(1);
            _currentHP -= _autoDamage;
            _damageTotal += _autoDamage;
            BossHP();
        }
    }

    public void ButtonDelete()
    {
        this.enabled = false;
    }

    /// <summary>
    /// 時間を止める処理
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeStopper()
    {
        _timeLimit++;
        _isTimeStop = true;
        yield return new WaitForSeconds(1);
        _isTimeStop = false;
    }

    /// <summary>
    /// 一秒に一回スキルボタンが押せるか抽選をする処理
    /// </summary>
    /// <returns></returns>
    IEnumerator ButtonEnabled()
    {
        while (_timeLimit > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                _skillBottons[i].enabled = false;
                _skillBottons[i].image.color = Color.gray;
                _buttonTexts[i].text = "";
            }

            for (int i = 0; i < 6; i++)
            {
                var number = Random.Range(1, 10);
                if (number == 1)
                {
                    _skillBottons[i].enabled = true;
                    _skillBottons[i].image.color = Color.red;
                    var n = Random.Range(1, 7);
                    switch (n)
                    {
                        case 0:
                            _buttonTexts[i].text = _devilList[0]._Name;
                            Debug.Log(_devilList[0]._Name);
                            break;
                        case 1:
                            _buttonTexts[i].text = _devilList[1]._Name;
                            Debug.Log (_devilList[1]._Name);
                            break;
                        case 2:
                            _buttonTexts[i].text = _devilList[2]._Name;
                            Debug.Log ( _devilList[2]._Name);
                            break;
                        case 3:
                            _buttonTexts[i].text = _devilList[3]._Name;
                            break;
                        case 4:
                            _buttonTexts[i].text = _devilList[4]._Name;
                            break;
                        case 5:
                            _buttonTexts[i].text = _devilList[5]._Name;
                            break;
                        case 6:
                            _buttonTexts[i].text = "Skill Failed";
                            break;
                    }//ボタンに振られたスキルが何か表示する
                }
            }
            if (_timeLimit <= 0)
            {
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}

