using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float maxHp;
    public float dmg;
    public int resourcesGranted;
    public ParticleSystem deathParticles;

    [HideInInspector]
    public float distanceToShelter;

    private NavMeshAgent navMeshAgent;
    private ParticleSystem dmgParticles;
    private GameObject shelter;
    private float currentHp;

    void Start()
    {
        currentHp = maxHp;
        shelter = GameObject.FindGameObjectWithTag("Shelter");
        navMeshAgent = GetComponent<NavMeshAgent>();
        dmgParticles = GetComponentInChildren<ParticleSystem>();
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.acceleration = moveSpeed;
    }

    void Update()
    {
        if (!GetComponent<NavMeshAgent>().isOnNavMesh) 
        {
          MoveToShelter();
        }
        distanceToShelter = Vector3.Distance(transform.position, shelter.transform.position);
        if(distanceToShelter < 2)
        {
            AtShelter(); 
        }
    }

    private void MoveToShelter()
    {
        transform.position = Vector3.MoveTowards(transform.position, shelter.transform.position, moveSpeed * Time.deltaTime);
        
        //TODO: Understand how this works so it can be written clearer
        Quaternion rotation = Quaternion.LookRotation(shelter.transform.position - transform.position);
        Quaternion bodyRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, bodyRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(float dmg)
    {
        dmgParticles.Play();
        currentHp -= dmg;
        if (currentHp <= 0)
        {
            Death();
        }
    }
    
    private void AtShelter()
    {
        shelter.GetComponent<Bunker>().TakeDmg(dmg);
        EnemyManager.Instance.enemiesAlive--;
        Destroy(gameObject);
    }

    private void Death()
    {
        if (deathParticles != null) 
        {
            ParticleSystem deathPs = Instantiate(deathParticles, gameObject.transform.position, Quaternion.identity);
            deathParticles.Play();
        }
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.EnemyDie, 2f);
        GameManager.Instance.GetResource(resourcesGranted);
        EnemyManager.Instance.enemiesAlive--;
        Destroy(gameObject);
    }
}
