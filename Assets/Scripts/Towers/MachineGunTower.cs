using Managers;
using UnityEngine;

namespace Towers
{
    public class MachineGunTower : Tower
    {
        //private List<ParticleSystem> muzzleFlashes = new();
        private int currentBarrel;
        private Transform gun;

        private void Start()
        {
            //muzzleFlashes = GetComponentsInChildren<ParticleSystem>().ToList();
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
            
            // muzzleFlashes[currentBarrel]?.Play();
            // currentBarrel = (currentBarrel + 1) % muzzleFlashes.Count;
            
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

            gun = transform.Find($"{stats.model[level].name}/Body/Gun");
            // body = transform.Find($"{model[level].name}/Base/Body");
            // barrel = transform.Find($"{model[level].name}/Base/Body/Barrel");

            // currentBarrel = 0;
            // muzzleFlashes = GetComponentsInChildren<ParticleSystem>().ToList();
        }
    }
}
