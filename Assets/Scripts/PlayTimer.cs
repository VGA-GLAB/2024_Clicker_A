using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;
    private float _hour = 0;
    private float _minute = 0;
    private float _second = 0;
    void Update()
    {
        _second += Time.deltaTime;
        if (_second >= 60)
        {
            _second = 0;
            _minute += 1;
        }
        if (_minute >= 60)
        {
            _minute = 0;
            _hour += 1;
        }
        _timerText.text = $"PlayTime{_hour.ToString("00")}:{_minute.ToString("00")}:{_second.ToString("00")}";
    }
}
