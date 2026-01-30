using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    public float projectileSpeed;
    public float dmg;
    void Start()
    {
        
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            MoveToTarget();
            if (transform.position == target.transform.position)
            {
                HitTarget();
            }
        }
    }
    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, projectileSpeed * Time.deltaTime);
    }

    private void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        enemy.TakeDamage(dmg);
        Destroy(gameObject);
    }
}
