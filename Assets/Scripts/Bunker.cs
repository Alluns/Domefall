using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Bunker : MonoBehaviour, IClickable
{
    private Vector3 extents = new Vector3(1,16,2);
    public int dmg;
    public LayerMask mask;
    public int evoPoints = 3;
    public float range;
    public float attackSpeed;
    public int armour;
    public int maxHp;
    public float currentHp;
    
    private float attackCooldown;
    private bool selected;
    
    private Vector3 aimDirection;
    private Vector3 rayDir;
    
    private Transform turret, barrel;
    private readonly List<ParticleSystem> muzzleFlashes = new();
    private int currentBarrel = 0;
    //private GameObject Highlight;
    private GameObject AimIndicator;
    private LineRenderer lineRenderer;

    public void Clicked() { }

    public void Selected()
    {
        //selected = true;
        //Highlight.SetActive(true);
    }

    public void DeSelected()
    {
        //selected = false;
        //Highlight.SetActive(false);
    }
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    private void Start()
    {
        turret = transform.Find("Bunker/Body/Cylinder/Gunbody");
        barrel = turret.transform.Find("Gunbarrel");
        //Highlight = GameObject.Find("Highlight");
        muzzleFlashes.AddRange(gameObject.GetComponentsInChildren<ParticleSystem>());
        attackCooldown = 1 / attackSpeed;
        currentHp = maxHp;
        GameManager.Instance.maxHp = maxHp;
        GameManager.Instance.currentHp = currentHp;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color (1, 0, 0, 0.3f);
        lineRenderer.endColor = new Color(1, 0, 0, 0.3f);

        //Highlight.SetActive(false);
    }
    private void Update()
    {
        if (Touch.activeTouches.Count > 0 && GameManager.Instance.currentState == GameManager.GameState.Playing)
        {
            lineRenderer.SetPosition(0, muzzleFlashes[0].transform.position);
            lineRenderer.SetPosition(1, new Vector3(transform.position.x + turret.transform.forward.x * range, muzzleFlashes[0].transform.position.y, transform.position.z + turret.transform.forward.z * range));
            Touch touch = Touch.activeTouches[0];
            Vector2 direction2D = (touch.screenPosition - touch.startScreenPosition);
            if (direction2D.magnitude > 75)
            {
                direction2D = direction2D.normalized;
                aimDirection = new Vector3(direction2D.x, 0, direction2D.y);
            }
            if (attackCooldown <= 0)
            {
                ShootInDirection(aimDirection);
                rayDir = aimDirection;
            }
        }
        else
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }
        RotateInDirection(aimDirection);
        attackCooldown -= Time.deltaTime;
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
            attackCooldown = 1 / attackSpeed;
        }
    }

    private void RotateInDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            turret.rotation = Quaternion.Slerp(turret.rotation, targetRot, Time.deltaTime * 100);
        }
    }

    public void TakeDmg(float dmg)
    {
        currentHp -= dmg;
        GameManager.Instance.currentHp = currentHp;
        if (currentHp <= 0) GameManager.Instance.SwitchState(GameManager.GameState.Lose);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawRay(transform.position, rayDir * 100); 
    //}
   
}
