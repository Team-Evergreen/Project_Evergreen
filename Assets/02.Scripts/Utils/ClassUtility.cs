using System;
using Utils.EnumType;
using System.Collections.Generic;

namespace Utils.ClassUtility
{
    // 무기 데이터 구조
    [Serializable]
    public class WeaponDataList
    {
        public List<WeaponData> Weapons;
    }

    [Serializable]
    public class WeaponData
    {
        public string weaponName;
        public string projectilePrefab;
        public string weaponAnim;
        public WeaponType weaponType;
        public EquipTarget equipTarget;
        public float speed;
        public float damage;
        public float fireRate;
        public float lifetime;
        public float range;
        public bool isMelee;
    }

    // 적 데이터 구조
    [Serializable]
    public class EnemyDataList
    {
        public List<EnemyData> Enemys;
    }

    [Serializable]
    public class EnemyData
    {
        public string enemyName;
        public float hp;
        public float speed;
        public float damage;
    }
}