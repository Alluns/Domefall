using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject levelMenu, optionsMenu, evolutionMenu;
    [SerializeField] private TMP_Text versionText;
    
    private void OnEnable()
    {
        SaveData saveData = JsonSave.LoadData();

        if (saveData.levelsCompleted.Count == 0 && saveData.evolutionPoints > 0)
        {
            evolutionMenu.SetActive(true);
        }
        
        versionText.text = $"Version: {Application.version}";
    }

    public void SwitchToLevelSelect()
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
