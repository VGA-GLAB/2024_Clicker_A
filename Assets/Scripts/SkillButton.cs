using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] Button[] _skillBottons;
    MiniGameQuestsManager _gameQuestsManager;
    void Start()
    {
        _gameQuestsManager = FindAnyObjectByType<MiniGameQuestsManager>();
        StartCoroutine(ButtonEnabled());
    }

    private void Update()
    {
        if (_gameQuestsManager.TimeLimit <= 0)
        {
            foreach (var button in _skillBottons)
            {
                button.enabled = false;
                button.image.color = new Color(0, 0, 0, 0);
                button.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }//タイマーが0になったらボタンを全て消す
    }

    public void ButtonDelete()
    {        
        this.enabled = false;
    }

    /// <summary>
    /// 一秒に一回スキルボタンが押せるか抽選をする処理
    /// </summary>
    /// <returns></returns>
    IEnumerator ButtonEnabled()
    {
        foreach (var button in _skillBottons)
        {
            button.enabled = false;
            button.image.color = Color.gray;
        }

        foreach (var button in _skillBottons)
        {
            var number = Random.Range(1, 10);
            if (number == 1)
            {
                button.enabled = true;
                button.image.color = Color.red;
            }
        }

        if (_gameQuestsManager.TimeLimit <= 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(ButtonEnabled());
    }
}

