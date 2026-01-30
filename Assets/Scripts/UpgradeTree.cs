using UnityEngine;

public class UpgradeTree : MonoBehaviour
{

    
    public void UpgradeHp()
    {
        GameObject.FindGameObjectWithTag("Shelter").GetComponent<Bunker>().maxHp += 10;
        Debug.Log("Upgraded Hp");
    }
    public void UpgradeDmg()
    {
        GameObject.FindGameObjectWithTag("Shelter").GetComponent<Bunker>().dmg += 10;
        Debug.Log("Upgraded Dmg");
    }
    public void UpgradeArmour()
    {
        GameObject.FindGameObjectWithTag("Shelter").GetComponent<Bunker>().armour += 50;
        Debug.Log("Upgraded Armour");
    }
    public void UpgradeAttackSpeed()
    {
        GameObject.FindGameObjectWithTag("Shelter").GetComponent<Bunker>().attackSpeed += 10;
        Debug.Log("Upgraded AttackSpeed");
    }
}
