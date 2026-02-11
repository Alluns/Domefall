using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Towers
{
    public class GroundTower : Tower
    {
        public GameObject trail;
        private List<ParticleSystem> muzzleFlashes = new();
        private int currentBarrel;
        public int bulletSpeed;
        private Transform body, barrel; 
        private void Start()
        {
            body = transform.Find($"{stats.model[level].name}/Base/Body");
            barrel = transform.Find($"{stats.model[level].name}/Base/Body/Barrel");

            muzzleFlashes = transform.Find(stats.model[level].name).GetComponentsInChildren<ParticleSystem>().ToList();
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
            
            body.rotation = Quaternion.Slerp(body.rotation, bodyRotation, Time.deltaTime * 5f);
        }

        protected override bool Attack()
        {
            if (!targetEnemy) return false;

            if (Vector3.Distance(targetEnemy.transform.position, transform.position) > stats.range)
            {
                targetEnemy = null;
                return false;
            }
            if (trail != null)
            {
                StartCoroutine(TrailAnimation(Instantiate(trail, muzzleFlashes[0].transform.position, Quaternion.identity), targetEnemy.transform.position));
            }
            muzzleFlashes[currentBarrel].Play();
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.GroundTower, 1.8f);
            currentBarrel = (currentBarrel + 1) % muzzleFlashes.Count;

            targetEnemy.TakeDamage(stats.damage);
            
            return true;
        }

        protected override void Upgrade()
        {
            base.Upgrade();
            
            body = transform.Find($"{stats.model[Mathf.Min(level, stats.model.Length - 1)].name}/Base/Body");
            barrel = transform.Find($"{stats.model[Mathf.Min(level, stats.model.Length - 1)].name}/Base/Body/Barrel");

            currentBarrel = 0;
            muzzleFlashes = transform.Find(stats.model[Mathf.Min(level, stats.model.Length - 1)].name).GetComponentsInChildren<ParticleSystem>().ToList();
        }

        private IEnumerator TrailAnimation(GameObject trail, Vector3 target)
        {
            Vector3 startPos = trail.transform.position;
            float distance = Vector3.Distance(startPos, target);
            float remainingDistance = distance;
            while (remainingDistance > 0)
            {
                remainingDistance -= Time.deltaTime * bulletSpeed;
                trail.transform.position = Vector3.Lerp(startPos, target, 1 - (remainingDistance / distance));
                yield return null;
            }
            trail.transform.position = target;
            //Destroy(trail.gameObject, trail.GetComponent<TrailRenderer>().time);
        }
    }
}
