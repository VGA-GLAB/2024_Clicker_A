using System;
using System.Collections;
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
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public DevilManager.Devil ChooseRarerity(string Rarerity) //ガチャ結果からレアリティに応じた使い魔選び
    {
        string _rarity = Rarerity;
        List<DevilManager.Devil> _targetList = _rarity switch
        {
            "Normal" => Normal,
            "Rare" => Rare,
            "SuperRare" => SRare,
            "Legend" => Legend,
            _ => null,
        };

        if(_targetList != null && _targetList.Count > 0)
        {
            int _index = Random.Range(0, _targetList.Count);
            return _targetList[_index];
        }

        return null;

    }

    //以下消費魔力量に応じたガチャ確率
    public string Gacha1()
    {
        float randomNum = Random.Range(0f, 100f);

        if(randomNum < 97)
        {
            return "Normal";
        }
        else if(randomNum == 97)
        {
            return "Rare";
        }
        else if(randomNum == 98)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public string Gacha2()
    {
        float randomNum = Random.Range(0f, 100f);

        if (randomNum < 90)
        {
            return "Normal";
        }
        else if (randomNum < 98)
        {
            return "Rare";
        }
        else if (randomNum == 98)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public string Gacha3()
    {
        float randomNum = Random.Range(0f, 100f);

        if (randomNum < 80)
        {
            return "Normal";
        }
        else if (randomNum < 98)
        {
            return "Rare";
        }
        else if (randomNum == 98)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public string Gacha4()
    {
        float randomNum = Random.Range(0f, 100f);

        if (randomNum < 70)
        {
            return "Normal";
        }
        else if (randomNum < 98)
        {
            return "Rare";
        }
        else if (randomNum == 98)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public string Gacha5()
    {
        float randomNum = Random.Range(0f, 100f);

        if (randomNum < 60)
        {
            return "Normal";
        }
        else if (randomNum < 98)
        {
            return "Rare";
        }
        else if (randomNum == 98)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public string Gacha6()
    {
        float randomNum = Random.Range(0f, 100f);

        if (randomNum < 50)
        {
            return "Normal";
        }
        else if (randomNum < 94)
        {
            return "Rare";
        }
        else if (randomNum < 99)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public string Gacha7()
    {
        float randomNum = Random.Range(0f, 100f);

        if (randomNum < 30)
        {
            return "Normal";
        }
        else if (randomNum < 89)
        {
            return "Rare";
        }
        else if (randomNum < 99)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public string Gacha8()
    {
        float randomNum = Random.Range(0f, 100f);

        if (randomNum < 20)
        {
            return "Normal";
        }
        else if (randomNum < 80)
        {
            return "Rare";
        }
        else if (randomNum < 98)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public string Gacha9()
    {
        float randomNum = Random.Range(0f, 100f);

        if (randomNum < 10)
        {
            return "Normal";
        }
        else if (randomNum < 60)
        {
            return "Rare";
        }
        else if (randomNum < 90)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public string Gacha10()
    {
        float randomNum = Random.Range(0f, 90f);

        if (randomNum < 20)
        {
            return "Rare";
        }
        else if (randomNum < 70)
        {
            return "SuperRare";
        }
        else
        {
            return "Legend";
        }
    }

    public void OutPutDevil()
    {
        BigInteger _pay = FindAnyObjectByType<ResourceManager>().Gacha();

        if (_pay < 1000)
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha1());
        }
        else if (_pay < 10000)
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha2());
        }
        else if (_pay < (BigInteger)1e5f)
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha3());
        }
        else if (_pay < (BigInteger)1e6f)
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha4());
        }
        else if (_pay < (BigInteger)1e7f)
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha5());
        }
        else if (_pay < (BigInteger)1e9f)
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha6());
        }
        else if (_pay < (BigInteger)1e10f)
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha7());
        }
        else if (_pay < (BigInteger)1e11f)
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha8());
        }
        else if (_pay < (BigInteger)1e12f)
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha9());
        }
        else
        {
            GachaSystem.Instance.ChooseRarerity(GachaSystem.Instance.Gacha10());
        }
    }
}
