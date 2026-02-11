using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Towers
{
    public class SweeperTower : Tower
    {
        private List<ParticleSystem> muzzleFlashes = new();
        private int currentBarrel;
        public GameObject trail;
        private Transform turret, breech;
        public int bulletSpeed;

        private void Start()
        {
            turret = transform.Find($"{stats.model[level].name}/Base/Turret");
            breech = turret.transform.Find("Breech");

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
            
            turret.rotation = Quaternion.Slerp(turret.rotation, bodyRotation, Time.deltaTime * 5f);
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
            currentBarrel = (currentBarrel + 1) % muzzleFlashes.Count;
            muzzleFlashes[currentBarrel].Play();
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.AirAndGround, 0.7f);

            targetEnemy.TakeDamage(stats.damage);
            
            return true;
        }

        protected override void Upgrade()
        {
            base.Upgrade();

            turret = transform.Find($"{stats.model[Mathf.Min(level, stats.model.Length - 1)].name}/Base/Turret");
            breech = turret.transform.Find("Breech");

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
