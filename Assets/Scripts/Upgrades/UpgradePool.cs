using System.Collections.Generic;
using UnityEngine;

public class UpgradePool : MonoBehaviour
{
    [System.Serializable]
    public class Upgrade
    {
        public enum StatTypes
        {
            dmg,
            range,
            attackSpeed,
        }
        public StatTypes stat;
        public int upgradeAmount;
        public int weight;
    }
    public List<Upgrade> upgradePool;

    private void Start()
    {

    }

    public List<Upgrade> GetRandomUpgrades(int upgradesNeeded)
    {
        List <Upgrade> result = new List<Upgrade>();
        int totalWeight = 0;
        int lastWeight = 0;
        List<Upgrade> upgradeList = new();
        upgradeList.AddRange(upgradePool);
        for(int i = 0; i< upgradePool.Count; i++)
        {
            totalWeight += upgradePool[i].weight;
        }
        for (int j = 0; j < upgradesNeeded; j++)
        {
            int rnd = Random.Range(1, totalWeight + 1);
            for (int i = 0; i < upgradeList.Count; i++)
            {
                if (rnd <= (upgradeList[i].weight + lastWeight))
                {
                    result.Add(upgradeList[i]);
                    totalWeight -= upgradeList[i].weight;
                    lastWeight = 0;
                    upgradeList.RemoveAt(i);
                    break;
                }
                else
                {
                    lastWeight += upgradeList[i].weight;
                }
            } 
        }

        return result;
    }
}
