using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TowerStats", menuName = "Scriptable Objects/TowerStats")]
    public class TowerStats : ScriptableObject
    {
        [Header("Base stats")]
        public float health = 100f;
        public float regeneration = 5f;
        public float armor;
        public float damage = 20f;
        public float criticalHitDamageMultiplier = 2f;
        public float criticalHitChance = 0.1f;
        public float attackSpeed = 1.0f;
        public float range = 50f;
        public float areaOfEffect;
        public float rotationSpeed = 30f;
        public LayerMask viableTargets;

        [Header("Advanced stats")]
        public float knockBack;
        public float pierce;
        public int ricochet;
        public int projectilePierce;
        public int extraProjectiles;
        public int additionalProjectile;
        public float resourcesMultiplier = 1f;
        
        [Header("Construction")]
        public float upgradeCost = 250.0f;
        public float upgradeCostScaling = 1.5f;
        public List<TowerUpgrade> upgradePool;

        [Header("Visuals")]
        public GameObject[] model;
    }
}
