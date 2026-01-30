using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using static GameManager;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Bunker : MonoBehaviour, IClickable
{
    Vector3 extents = new Vector3(2,16,2);
    public int dmg;
    public LayerMask mask;
    public float range;
    public float attackSpeed;
    public int armour;
    public int maxHp;
    public float currentHp;
    public float maxHeat;
    public float currentHeat = 0;
    private float attackCooldown;
    private Vector3 rayDir;
    private bool selected;
    private Transform cylinder;
    private readonly List<ParticleSystem> muzzleFlashes = new();
    private int currentBarrel = 0;
    private Vector3 aimDirection;
    public int evoPoints = 3;
    
    public void Clicked() { }

    public void Selected()
    {
        selected = true;
    }

    public void DeSelected()
    {
        selected = false;
    }
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    private void Start()
    {
        attackCooldown = 1 / attackSpeed;
        cylinder = transform.Find("Parent").Find("Body").Find("Cylinder Parent");
        muzzleFlashes.AddRange(gameObject.GetComponentsInChildren<ParticleSystem>());
        currentHp = maxHp;
        GameManager.Instance.maxHp = maxHp;
        GameManager.Instance.currentHp = currentHp;
    }
    private void Update()
    {
        if (Touch.activeTouches.Count > 0 && selected)
        {
            Touch touch = Touch.activeTouches[0];
            Vector2 direction2D = (touch.screenPosition - touch.startScreenPosition).normalized;
             aimDirection = new Vector3(direction2D.x, 0, direction2D.y);
            if (attackCooldown <= 0 && currentHeat < maxHeat)
            {

                ShootInDirection(aimDirection);
                rayDir = aimDirection;
            }
        }

        RotateInDirection(aimDirection);
        attackCooldown -= Time.deltaTime;
        if (currentHeat > 0 && attackCooldown <= 0)
        {
            currentHeat -= Time.deltaTime * 4;
        }
    }

    private void ShootInDirection(Vector3 direction)
    {
        if(Physics.BoxCast(transform.position, extents, direction, out RaycastHit hit, Quaternion.identity, range, mask))
        {
            Enemy target = hit.transform.gameObject.GetComponent<Enemy>();
            target.TakeDamage(dmg);
            muzzleFlashes[currentBarrel].Play();
            currentBarrel++;
            currentBarrel %= muzzleFlashes.Count;
            currentHeat++;
            attackCooldown = 1 / attackSpeed;
        }
    }

    private void RotateInDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            cylinder.rotation = Quaternion.Slerp(cylinder.rotation, targetRot, Time.deltaTime * 100);
        }
    }

    public void TakeDmg(float dmg)
    {
        currentHp -= dmg;
        GameManager.Instance.currentHp = currentHp;
        if (currentHp <= 0) GameManager.Instance.SwitchState(GameState.Lose);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, rayDir * 100); 
    }
}
