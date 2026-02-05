using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Towers
{
    public class MissileTurret : Tower
    {
        private List<ParticleSystem> muzzleFlashes = new();
        private int currentBarrel;
        
        private void Start()
        {
            muzzleFlashes = transform.Find(stats.model[level].name).GetComponentsInChildren<ParticleSystem>().ToList();
        }
        
        protected override void Update()
        {
            base.Update();
            
            if (!targetEnemy) targetEnemy = FindTarget();
        }

        protected override bool Attack()
        {
            if (!targetEnemy) return false;

            if (Vector3.Distance(targetEnemy.transform.position, transform.position) > stats.range)
            {
                targetEnemy = null;
                return false;
            }
            
            muzzleFlashes[currentBarrel].Play();
            currentBarrel = (currentBarrel + 1) % muzzleFlashes.Count;
            
            targetEnemy.TakeDamage(stats.damage);
            
            return true;
        }

        protected override void Upgrade()
        {
            base.Upgrade();

            currentBarrel = 0;
            muzzleFlashes = transform.Find(stats.model[level].name).GetComponentsInChildren<ParticleSystem>().ToList();
        }
    }
}
