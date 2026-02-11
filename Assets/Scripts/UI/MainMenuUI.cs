using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject levelMenu, optionsMenu, evolutionMenu; 
    
    private void OnEnable()
    {
        SaveData saveData = JsonSave.LoadData();

        if (saveData.levelsCompleted.Count == 0 && saveData.evolutionPoints > 0)
        {
            evolutionMenu.SetActive(true);
        }
    }

    private void SwitchToLevelSelect()
    {
        evolutionMenu.SetActive(false);
        optionsMenu.SetActive(false);
        levelMenu.SetActive(true);
    }
    
    public void SwitchToOptions()
    {
        optionsMenu.SetActive(true);
    }
}
