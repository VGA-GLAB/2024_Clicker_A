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
        _beforeUpgradeArray = FindObjectsOfType<UpGrade>().ToList();
        foreach(var component in _beforeUpgradeArray)
        {
            component.OnUpgrade += () =>
            {
                _afterUpgradeArray.Add(component);
                _beforeUpgradeArray.Remove(component);
                component.gameObject.SetActive(false);
            };
        }
    }

    void Update()
    {
        
    }
}
