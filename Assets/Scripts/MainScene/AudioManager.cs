using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SymbolAudioType
    {
        HighPay1,
        HighPay2,
        HighPay3,
        HighPay4,
        LowPay,
        Wild
    }
    public static AudioManager Instance;

    [Header("Spin Sounds")]
    public AudioClip spinStartClip;
    public AudioClip spinStopClip;

    [Header("Win Sounds")]
    public AudioClip highPayClip1;
    public AudioClip highPayClip2;
    public AudioClip highPayClip3;
    public AudioClip highPayClip4;
    public AudioClip lowPayClip;
    public AudioClip wildClip;

    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        sfxSource = gameObject.GetComponent<AudioSource>();
        sfxSource.playOnAwake = false;
    }

    public void PlaySpinStart()
    {
        PlayOneShot(spinStartClip);
    }

    public void PlaySpinStop()
    {
        PlayOneShot(spinStopClip);
    }

    public void PlayWin(SymbolAudioType type)
    {
        switch (type)
        {
            case SymbolAudioType.HighPay1:
                PlayOneShot(highPayClip1);
                break;
            case SymbolAudioType.HighPay2:
                PlayOneShot(highPayClip2);
                break;
            case SymbolAudioType.HighPay3:
                PlayOneShot(highPayClip3);
                break;
            case SymbolAudioType.HighPay4:
                PlayOneShot(highPayClip4);
                break;
            case SymbolAudioType.LowPay:
                PlayOneShot(lowPayClip);
                break;
            case SymbolAudioType.Wild:
                PlayOneShot(wildClip);
                break;
        }
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
