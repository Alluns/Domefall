using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public enum Sfx
    {
        AirAndGround,
        AirAndGround2,
        AirAndGround3,
        AirTower,
        AirTower2,
        BasicShot,
        CrateDestroyed,
        CrateHit,
        EnemyDie,
        EnemyDie2,
        EnemyDie3,
        GroundTower,
        GroundTower2,
        GroundTower3,
        TowerUpgrade,
        TowerUpgrade2,
        TowerUpgrade3,
    }

    public enum Bgm
    {
        BlackSea,
        Redalalert5,
        none
    }

    public static SoundManager Instance;
    private SaveData saveData;

    [Header ("Change bgm here")]
    public Bgm music;


    [SerializeField]
    [Header ("DO NOT TOUCH")]
    private AudioClip[] sfxClips;
    [SerializeField]
    private AudioClip[] bgmClips;
    private AudioSource sfxSource, bgmSource;

    private float sfxVolume;
    private float bgmVolume;

    private void Awake()
    {
        Instance = this;
        sfxSource = gameObject.AddComponent<AudioSource>();
        bgmSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        saveData = JsonSave.LoadData();
        sfxVolume = saveData.sfxVolume;
        bgmVolume = saveData.bgmVolume;
        if (music != Bgm.none)
        {
            PlayBgm(music);
        }
    }

    public void PlaySfx(Sfx sound, float volumeMult)
    {
        Instance.sfxSource.PlayOneShot(sfxClips[(int)sound], sfxVolume * volumeMult);
    }

    public void PlayBgm(Bgm bgm)
    {
        Instance.bgmSource.clip = Instance.bgmClips[(int)bgm];
        Instance.bgmSource.volume = bgmVolume;
        Instance.bgmSource.loop = true;
        Instance.bgmSource.Play();
    }
}
