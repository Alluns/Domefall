using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DELETEBEFOREPUSHTowerRange : MonoBehaviour
{
    public float TowerRange;
    public bool Air;
    public bool Ground;
    public bool AirGround;
    public bool Basic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (Air)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, TowerRange);
        }
        if (Ground)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, TowerRange);
        }
        if (AirGround)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, TowerRange);
        }
        if (Basic)
        {
            Gizmos.color = Color.rebeccaPurple;
            Gizmos.DrawWireSphere(transform.position, TowerRange);
        }
    }
}
