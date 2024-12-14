using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BossParameter")]
public class BossParameter : ScriptableObject
{
    [Header("ボスのID")] public int Id;
    [Header("ボスの最大HP")] public int BossMaxHP;
    [Header("ボスの見た目")] public Sprite BossImage;
}
