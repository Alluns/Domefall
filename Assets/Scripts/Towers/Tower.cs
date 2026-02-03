using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using System.Linq;
using Managers;
using System;

namespace Towers
{
    public abstract class Tower : MonoBehaviour, IClickable
    {
        public TowerStats towerStats;
        protected TowerStats stats;

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] protected bool displayRange;
        [SerializeField] protected bool displayTarget;
#endif

        protected List<TowerUpgrade> upgrades = new();

        public List<TowerUpgrade> Upgrades
        {
            get => upgrades;
        }

        protected Enemy targetEnemy;
        protected int level;
        protected float attackCooldown;

        private void Awake()
        {
            stats = Instantiate(towerStats);
        }

        private void Start()
        {
            ApplyAllUpgrades();
        }

        protected virtual void Update()
        {
            attackCooldown -= Time.deltaTime;

            if (!(attackCooldown <= 0)) return;

            if (Attack())
            {
                attackCooldown = 1.0f / stats.attackSpeed;
            }
        }

        protected abstract bool Attack();

        protected virtual void Upgrade()
        {
            level++;
            
            UIManager.Instance.OpenUI(Menus.UpgradeMenu);
            GameManager.Instance.SwitchState(GameManager.GameState.Upgrade);

            if (level < stats.model.Length)
            {
                Destroy(transform.GetChild(0).gameObject);
            
                Instantiate(stats.model[level], transform).name = stats.model[level].name;
            }
            
            
        }

        public virtual void AddUpgrade(TowerUpgrade upgrade)
        {
            upgrades.Add(upgrade);
            ApplyAllUpgrades();
            
            GameManager.Instance.SwitchState(GameManager.GameState.Playing);
        }

        private void ApplyAllUpgrades()
        {
            stats = Instantiate(towerStats);
            
            foreach (UpgradeAttribute attribute in upgrades.SelectMany(upgrade => upgrade.upgradeAttributes))
            {
                switch (attribute.upgradeType)
                {
                    case UpgradeType.MaxHealth:
                        stats.health += attribute.additive;
                        stats.health *= attribute.multiplicative;
                        break;
                    case UpgradeType.Regen:
                        stats.regeneration += attribute.additive;
                        stats.regeneration *= attribute.multiplicative;
                        break;
                    case UpgradeType.Damage:
                        stats.damage += attribute.additive;
                        stats.damage *= attribute.multiplicative;
                        break;
                    case UpgradeType.AttackSpeed:
                        stats.attackSpeed += attribute.additive;
                        stats.attackSpeed *= attribute.multiplicative;
                        break;
                    case UpgradeType.Range:
                        stats.range += attribute.additive;
                        stats.range *= attribute.multiplicative;
                        break;
                    case UpgradeType.Ricochet:
                        stats.ricochet += (int) attribute.additive;
                        stats.ricochet *= (int) attribute.multiplicative;
                        break;
                    case UpgradeType.ExtraShot:
                        stats.extraProjectiles += (int) attribute.additive;
                        stats.extraProjectiles *= (int) attribute.multiplicative;
                        break;
                    case UpgradeType.AdditionalProjectiles:
                        stats.additionalProjectile += (int) attribute.additive;
                        stats.additionalProjectile *= (int) attribute.multiplicative;
                        break;
                    case UpgradeType.Resources:
                        stats.resourcesMultiplier += attribute.additive;
                        stats.resourcesMultiplier *= attribute.multiplicative;
                        break;
                    case UpgradeType.Knockback:
                        stats.knockBack += attribute.additive;
                        stats.knockBack *= attribute.multiplicative;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected virtual Enemy FindTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, stats.range, stats.viableTargets);
            Enemy[] enemiesInRange = (from collider in hitColliders where collider.CompareTag("Enemy") select collider.gameObject.GetComponent<Enemy>()).ToArray();

            if (enemiesInRange.Length == 0) return null;

            float shortestDistance = enemiesInRange.Min(e => e.distanceToShelter);
            
            return enemiesInRange.First(e => Mathf.Approximately(e.distanceToShelter, shortestDistance));
        }

        public void Clicked()
        {
            if (GameManager.Instance.currentResources >= stats.upgradeCost)
            {
                GameManager.Instance.currentResources -= Mathf.FloorToInt(stats.upgradeCost);
                stats.upgradeCost *= stats.upgradeCostScaling;
                
                Upgrade();
            }
        }

        public void Selected() { }

        public void DeSelected() { }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (displayRange)
            {
                Gizmos.color = Color.forestGreen;
                Gizmos.DrawWireSphere(transform.position, stats.range);
            }

            if (displayTarget)
            {
                Gizmos.color = Color.darkOrange;
                Gizmos.DrawRay(transform.position, targetEnemy.transform.position - transform.position);
            }
        }
#endif
    }
}
