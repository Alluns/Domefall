using System.Linq;
using UnityEngine;

namespace Towers
{
    public abstract class Tower : MonoBehaviour, IClickable
    {
        [Header("Attack")]
        public float damage = 10.0f;
        public float attackSpeed = 1.0f;
        public float range = 15.0f;
        [SerializeField] protected LayerMask viableTargets;
        
        [Header("Construction")]
        [SerializeField] protected float upgradeCost = 200.0f;
        [SerializeField] protected float upgradeCostScaling = 1.5f;

        [Header("Visuals")]
        [SerializeField] protected float rotationSpeed;
        [SerializeField] protected GameObject[] model;

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] protected bool displayRange;
        [SerializeField] protected bool displayAim;
#endif

        protected int level;
        protected Enemy targetEnemy;
        
        private float attackCooldown;

        protected virtual void Update()
        {
            attackCooldown -= Time.deltaTime;

            if (!(attackCooldown <= 0)) return;

            if (Attack())
            {
                attackCooldown = 1.0f / attackSpeed;
            }
        }

        protected abstract bool Attack();

        protected virtual void Upgrade()
        {
            if (level >= model.Length - 1) return;

            level++;

            Destroy(transform.GetChild(0).gameObject);

            Instantiate(model[level], transform).name = model[level].name;
        }

        protected virtual Enemy FindTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, viableTargets);
            Enemy[] enemiesInRange = (from collider in hitColliders where collider.CompareTag("Enemy") select collider.gameObject.GetComponent<Enemy>()).ToArray();

            if (enemiesInRange.Length == 0) return null;

            float shortestDistance = enemiesInRange.Min(e => e.distanceToShelter);
            
            return enemiesInRange.First(e => Mathf.Approximately(e.distanceToShelter, shortestDistance));
        }

        public void Clicked()
        {
            if (GameManager.Instance.currentResources >= upgradeCost)
            {
                GameManager.Instance.currentResources -= Mathf.FloorToInt(upgradeCost);
                upgradeCost *= upgradeCostScaling;
                
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
                Gizmos.DrawWireSphere(transform.position, range);
            }
        }
#endif
    }
}
