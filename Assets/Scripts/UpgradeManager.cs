using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    List<UpGrade> _beforeUpgradeArray;
    List<UpGrade> _afterUpgradeArray = new List<UpGrade>();
    void Start()
    {
        _beforeUpgradeArray = FindObjectsByType<UpGrade>(FindObjectsSortMode.None).ToList();
        for (int i = 0; i < _beforeUpgradeArray.Count; i++)
        {
            UpGrade component = _beforeUpgradeArray[i];
            component.OnUpgrade += () =>
            {
                _afterUpgradeArray.Add(component);
                _beforeUpgradeArray.Remove(component);
                component.gameObject.SetActive(false);
                PlayerPrefs.SetString(component.gameObject.name, component.UpGradeProductName);
            };
            if (PlayerPrefs.HasKey(component.gameObject.name))
            {
                component.OnUpgrade.Invoke();
            }
        }
    }
}
