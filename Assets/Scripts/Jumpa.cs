using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jumpa : UpgradeTree
{
    public Sprite active;
    public Sprite inactive;
    public Sprite activatable;
    public JumpaState jumpaState;
    
    public enum JumpaState
    {
        Active,
        Inactive,
        Activatable,
    }

    public List<Jumpa> childJumpa = new List<Jumpa>();

    public void OnUpgrade()
    {
        if (GameObject.FindGameObjectWithTag("Shelter").GetComponent<Bunker>().evoPoints <= 0)
        {
            Debug.Log("Not enough Evolution Points");
            return;
        }
        else
        {
            if (jumpaState == JumpaState.Activatable)
            {
                foreach (Jumpa jumpa in childJumpa)
                {
                    jumpa.jumpaState = JumpaState.Activatable;
                    jumpa.gameObject.GetComponent<Image>().sprite = activatable;

                }
                GameObject.FindGameObjectWithTag("Shelter").GetComponent<Bunker>().evoPoints--;
                gameObject.GetComponent<Image>().sprite = active;
            }
            else Debug.Log("Not unlocked yet");

        }
        
    }
}
