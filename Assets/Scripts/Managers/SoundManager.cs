using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference Mlevel01 { get; private set; }
    private EventInstance _Mlevel01;
    [field: SerializeField] public EventReference Mlevel02 { get; private set; }
    private EventInstance _Mlevel02;
    [field: SerializeField] public EventReference SFXbasic { get; private set; }
    private EventInstance _SFXbasic;
    [field: SerializeField] public EventReference SFXAirGround { get; private set; }
    private EventInstance _SFXAirGround;
    [field: SerializeField] public EventReference SFXAir { get; private set; }
    private EventInstance _SFXAir;
    [field: SerializeField] public EventReference SFXGround { get; private set; }
    private EventInstance _SFXGround;
    [field: SerializeField] public EventReference SFXEnemyDie { get; private set; }
    private EventInstance _SFXEnemyDie;
    [field: SerializeField] public EventReference SFXTowerUpgrade { get; private set; }
    private EventInstance _TowerUpgrade;
    [field: SerializeField] public EventReference Crates{ get; private set; }
    private EventInstance _Crates;
    [field: Header("Player SFX")]

    [field: Header("Ambiance")]


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.Mlevel01, transform.position);
    }
    private void FixedUpdate()
    {
        //UpdateSound01();
        //InitializeMusic(Music);
    }

    private void UpdateLevel01()
     {
         PLAYBACK_STATE playbackState;
        _Mlevel01.getPlaybackState(out playbackState);
         if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
         {
            _Mlevel01.start();
         }
         else
         {
            _Mlevel01.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
         }
     }
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
