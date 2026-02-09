using System.Collections.Generic;

public class SaveData
{
    public int evolutionPoints = 0;
    public float bgmVolume = 1.0f;
    public float sfxVolume = 1.0f;
    
    public List<EvolutionNode.Evolution> evolutions = new();
    public List<int> levelsCompleted = new();
}
