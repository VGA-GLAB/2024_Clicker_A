using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;
    private int _hour = 0;
    private int _minute = 0;
    private float _second = 0;
    void Update()
    {
        _second += Time.deltaTime;
        if (_second >= 60)
        {
            _second -= 60;
            _minute++;
            if (_minute >= 60)
            {
                _minute = 0;
                _hour++;
            }
        }
        _timerText.text = $"PlayTime{_hour.ToString("00")}:{_minute.ToString("00")}:{_second.ToString("00")}";
    }
}
