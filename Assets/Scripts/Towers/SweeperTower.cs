using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Towers
{
    public class SweeperTower : Tower
    {
        private List<ParticleSystem> muzzleFlashes = new();
        private int currentBarrel;
        
        //private Transform turret, breech;

        private void Start()
        {
            //turret = transform.Find($"{model[level].name}/Base");
            //breech = turret.transform.Find("Breech");
            
            muzzleFlashes = GetComponentsInChildren<ParticleSystem>().ToList();
        }

        protected override void Update()
        {
            base.Update();

            if (!targetEnemy) targetEnemy = FindTarget();

            if (!targetEnemy) return;

            // Rotation shenanigans
            // TODO: When the local rotations have been fixed by the artists the body / barrel then rotate them instead
            Quaternion rotation = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
            Quaternion bodyRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, bodyRotation, Time.deltaTime * 5f);
        }

        protected override bool Attack()
        {
            if (!targetEnemy) return false;

            if (Vector3.Distance(targetEnemy.transform.position, transform.position) > range)
            {
                targetEnemy = null;
                return false;
            }
            
            muzzleFlashes[currentBarrel].Play();
            currentBarrel = (currentBarrel + 1) % muzzleFlashes.Count;
            
            targetEnemy.TakeDamage(damage);
            
            return true;
        }

        protected override void Upgrade()
        {
            base.Upgrade();
            
            //turret = transform.Find($"{model[level].name}/Base");
            //breech = turret.transform.Find("Breech");
            
            muzzleFlashes = GetComponentsInChildren<ParticleSystem>().ToList();
        }
    }
}
