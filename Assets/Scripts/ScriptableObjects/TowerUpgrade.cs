using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable] public class UpgradeAttribute
    {
        public UpgradeType upgradeType;
        public float additive;
        public float multiplicative = 1;
    }
    
    [CreateAssetMenu(fileName = "TowerUpgrade", menuName = "Scriptable Objects/TowerUpgrade")]
    public class TowerUpgrade : ScriptableObject
    {
        [Header("Upgrade")]
        public string upgradeName;
        public string description;

        [Header("Attributes")]
        public List<UpgradeAttribute> upgradeAttributes;
    }
    
    public enum UpgradeType
    {
        MaxHealth,
        Regen,
        Damage,
        AttackSpeed,
        Range,
        Ricochet,
        ExtraShot,
        AdditionalProjectiles,
        Resources,
        Knockback,
    }
}
