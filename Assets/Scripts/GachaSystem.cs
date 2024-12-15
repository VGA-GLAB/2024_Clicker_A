using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GachaSystem : MonoBehaviour
{
    public List<DevilManager.Devil> Normal;  //ノーマル使い魔格納用
    public List<DevilManager.Devil> Rare;    //レア使い魔格納用
    public List<DevilManager.Devil> SRare;   // Sレア使い魔格納用
    public List<DevilManager.Devil> Legend; //レジェンド使い魔格納用

    public static GachaSystem Instance;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public DevilManager.Devil ChooseRarity(string rarity) //ガチャ結果からレアリティに応じた使い魔選び
    {
        List<DevilManager.Devil> targetList = rarity switch
        {
            "Normal" => Normal,
            "Rare" => Rare,
            "SuperRare" => SRare,
            "Legend" => Legend,
            _ => null,
        };

        if (targetList != null && targetList.Count > 0)
        {
            int index = Random.Range(0, targetList.Count);
            return targetList[index];
        }
        return null;
    }

    //以下消費魔力量に応じたガチャ確率
    public string ChooseGacha(int normalChance, int rareChance, int superRareChance) => Random.Range(1, 101) switch
    {
        var i when i <= normalChance => "Normal",
        var i when i <= normalChance + rareChance => "Rare",
        var i when i <= normalChance + rareChance + superRareChance => "SuperRare",
        _ => "Legend"
    };

    public void OutPutDevil()
    {
        BigInteger pay = FindAnyObjectByType<ResourceManager>().Gacha();
        (int N, int R, int SR) = pay switch
        {
            var i when i <= (BigInteger)1e3f => (97, 1, 1),
            var i when i <= (BigInteger)1e4f => (90, 8, 1),
            var i when i <= (BigInteger)1e5f => (80, 18, 1),
            var i when i <= (BigInteger)1e6f => (70, 28, 1),
            var i when i <= (BigInteger)1e7f => (60, 38, 1),
            var i when i <= (BigInteger)1e8f => (50, 44, 5),
            var i when i <= (BigInteger)1e9f => (30, 59, 10),
            var i when i <= (BigInteger)1e10f => (20, 60, 18),
            var i when i <= (BigInteger)1e11f => (10, 50, 30),
            _ => (0, 20, 50)
        };
        ChooseGacha(N, R, SR);
    }
}
