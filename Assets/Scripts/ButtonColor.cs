using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void ChangeColor()
    {
        image.color = Color.gray;
    }
}
