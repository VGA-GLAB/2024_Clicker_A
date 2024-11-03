using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceIndication : MonoBehaviour
{
    TextMeshProUGUI _resourceTxt;
    ResourceManager _resourceManager;
    private void Start()
    {
        _resourceManager = FindAnyObjectByType<ResourceManager>();
        _resourceTxt = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
       _resourceTxt.text = _resourceManager.Resource.ToString();
    }
}
