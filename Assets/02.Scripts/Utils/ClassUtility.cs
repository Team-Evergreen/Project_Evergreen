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
        public string weaponName;       // 무기 이름
        public string projectilePrefab; // 총알 프리팹
        public string weaponAnim;       // 무기 애니메이션 이름
        public WeaponType weaponType;   // 무기 타입
        public EquipTaype equipTarget;  // 무기 종류
        public float speed;             // 투사체 속도
        public float damage;            // 데미지
        public float fireRate;          // 발사 속도
        public float lifetime;          // 투사체 생존 시간
        public float range;             // 사거리 (근접 무기인 경우 공격 범위로 사용)
        public bool isMelee;            // 근접 무기 여부
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
        public int id;
        public string enemyName;
        public float maxHP;
        public float speed;
        public float damage;
        public float scale;
    }
}