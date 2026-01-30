using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Towers
{
    public class GroundTower : Tower
    {
        public TrailRenderer trail;
        private List<ParticleSystem> muzzleFlashes = new();
        private int currentBarrel;

        // private Transform body, barrel;
        
        private void Start()
        {
            // body = transform.Find("Tier1/Base/Body");
            // barrel = transform.Find("Tier1/Base/Body/Barrel");

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

            StartCoroutine(TrailAnimation(Instantiate(trail, muzzleFlashes[0].transform.position, Quaternion.identity), targetEnemy.transform.position));
            muzzleFlashes[currentBarrel].Play();
            currentBarrel = (currentBarrel + 1) % muzzleFlashes.Count;
            
            targetEnemy.TakeDamage(damage);
            
            return true;
        }

        protected override void Upgrade()
        {
            base.Upgrade();
            
            // body = transform.Find($"{model[level].name}/Base/Body");
            // barrel = transform.Find($"{model[level].name}/Base/Body/Barrel");
            
            muzzleFlashes = GetComponentsInChildren<ParticleSystem>().ToList();
        }

        private IEnumerator TrailAnimation(TrailRenderer trail, Vector3 target)
        {
            Vector3 startPos = trail.transform.position;
            int bulletSpeed = 300;
            float distance = Vector3.Distance(startPos, target);
            float remainingDistance = distance;
            while (remainingDistance > 0)
            {
                remainingDistance -= Time.deltaTime * bulletSpeed;
                trail.transform.position = Vector3.Lerp(startPos, target, 1 - (remainingDistance / distance));
                yield return null;
            }
            trail.transform.position = target;
            Destroy(trail.gameObject, trail.time);
        }
    }
}
