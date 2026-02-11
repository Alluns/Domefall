using Managers;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Towers
{
    public class MachineGunTower : Tower
    {
        private List<ParticleSystem> muzzleFlashes = new();
        private int currentBarrel;
        private Transform gun;
        private Transform body, barrel;

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
    }
}
