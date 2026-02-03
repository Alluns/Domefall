using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference Mlevel01 { get; private set; }
    private EventInstance _Mlevel01;
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
