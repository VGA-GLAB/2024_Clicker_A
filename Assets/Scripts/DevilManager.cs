using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilManager : MonoBehaviour
{
    [System.Serializable]
    public class Devil
    {
        public string Name;  //使い魔の名前
        public bool IsActive;　　//アクティブ状態
        public int Level;   //レベル
        public RarityType Rarity;　//レアリティ
        public float Productivity;　//生産量
        public float Interval;　　//生産間隔
        public Skills Skill;　　　//スキル
        public float ClickPower;　//クリック力
        public float AutoClickPower;
        public float LevelupClickPower;　　　//レベルアップごとのクリック力上昇
        public float LevelupAutoClickPower;　//レベルアップごとの自動クリック上昇


        public Devil(string _name, RarityType rarity, float productivity, float interval)
        {
            Name = _name;
            IsActive = false;
            Level = 1;
            Rarity = rarity;
            Productivity = productivity;
            Interval = interval;
            ClickPower = 0;
            AutoClickPower = 0;
            LevelupClickPower = 0;
            LevelupAutoClickPower = 0;
        }
    }
    public enum RarityType 
    { 
        Normal, 
        Rare, 
        SRare,
        Legend
    }
    public enum Skills 
    { 
        FlashDamage,     //瞬間ダメージ
        AutoDamage,      //自動ダメージ
        TimeDamage,      //時間ダメージ
        FlashDmUp,       //瞬時クリック力増加
        AutoDmUp,        //瞬間自動クリック増加
        StopTime,        //時間増加
        ChangeClick,     //クリック力変換
        ChangeAutoClick  //自動クリック変換
    }

    public List<Devil> Devils = new List<Devil>();
    private int _maxDevils = 6;


}
