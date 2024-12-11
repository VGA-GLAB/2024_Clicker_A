using TMPro;
using UnityEngine;

public class PlayTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;
    private int _hour = 0;
    private int _minute = 0;
    private float _second = 0;
    private bool _isTimer = true;
    void Update()
    {
        if (_isTimer)
        {
            _second += Time.deltaTime;
        }
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
        if (_hour >= 999 && _minute >= 59 && _second >= 59)
        {
            _isTimer = false;
        }
        _timerText.text = $"PlayTime{_hour.ToString("00")}:{_minute.ToString("00")}:{_second.ToString("00")}";
    }
}
