using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Towers
{
    public class MachineGunTower : Tower
    {
        public GameObject trail;
        private List<ParticleSystem> muzzleFlashes = new();
        private int currentBarrel;
        private Transform gun;
        private Transform body, barrel;
        public int bulletSpeed;


        private void Start()
        {
            muzzleFlashes = transform.Find(stats.model[level].name).GetComponentsInChildren<ParticleSystem>().ToList();
            gun = transform.Find($"{stats.model[level].name}/Body/Gun");
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
            gun.rotation = Quaternion.Slerp(gun.rotation, bodyRotation, Time.deltaTime * 5f);
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
            if (trail != null)
            {
                StartCoroutine(TrailAnimation(Instantiate(trail, muzzleFlashes[0].transform.position, Quaternion.identity), targetEnemy.transform.position));
            }
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.BasicShot, 1.5f);
            currentBarrel = (currentBarrel + 1) % muzzleFlashes.Count;
            
            targetEnemy.TakeDamage(stats.damage);
            
            return true;
        }
        
        protected override void Upgrade()
        {
            if (level == 1)
            {
                UIManager.Instance.OpenUI(Menus.TowerSelectionMenu);
                GameManager.Instance.SwitchState(GameManager.GameState.Upgrade);
                return;
            }
            
            base.Upgrade();

            gun = transform.Find($"{stats.model[Mathf.Min(level, stats.model.Length)].name}/Body/Gun");
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
