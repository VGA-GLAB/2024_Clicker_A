using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilManager : MonoBehaviour
{
    [System.Serializable]
    public class Devils
    {
        public string _Name;  //使い魔の名前
        public bool _isActive;　　//アクティブ状態
        public int _level;   //レベル
        public RarityType _rarity;　//レアリティ
        public float _productivity;　//生産量
        public float _interval;　　//生産間隔
        public Skills _skill;　　　//スキル
        public float _clickPower;　//クリック力
        public float _autoClickPower;
        public float _levelupClickPower;　　　//レベルアップごとのクリック力上昇
        public float _levelupAutoClickPower;　//レベルアップごとの自動クリック上昇


        public Devils(string _name, RarityType rarity, float productivity, float interval)
        {
            _Name = _name;
            _isActive = false;
            _level = 1;
            _rarity = rarity;
            _productivity = productivity;
            _interval = interval;
            _clickPower = 0;
            _autoClickPower = 0;
            _levelupClickPower = 0;
            _levelupAutoClickPower = 0;
        }
    }
    public enum RarityType { Normal, Rare, SRare, Legend}
    public enum Skills { FlashDamage, AutoDamage, TimeDamage, FlashDmUp, AutoDmUp, StopTime, ChangeClick, ChangeAutoClick}

    public List<Devils> _devils = new List<Devils>();
    public int _maxDevils = 6;


}
