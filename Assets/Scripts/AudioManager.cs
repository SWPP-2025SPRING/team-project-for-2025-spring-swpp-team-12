using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip clearSound;

    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    
    private AudioSource audioSource;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            InitializeAudioSource();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 자동 생성을 위한 정적 메서드
    public static void EnsureInstance()
    {
        if (Instance == null)
        {
            GameObject audioManagerObj = new GameObject("AudioManager");
            audioManagerObj.AddComponent<AudioManager>();
        }
    }

    private void InitializeAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = sfxVolume;
    }

    // 점프 사운드 재생
    public void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound, sfxVolume);
            Debug.Log("[AudioManager] Jump sound played");
        }
        else
        {
            Debug.LogWarning("[AudioManager] Jump sound is not assigned!");
        }
    }

    // 클리어 사운드 재생
    public void PlayClearSound()
    {
        if (clearSound != null)
        {
            audioSource.PlayOneShot(clearSound, sfxVolume);
            Debug.Log("[AudioManager] Clear sound played");
        }
        else
        {
            Debug.LogWarning("[AudioManager] Clear sound is not assigned!");
        }
    }

    // 일반적인 사운드 재생 (확장 가능)
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume * sfxVolume);
        }
    }

    // 볼륨 설정
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        audioSource.volume = sfxVolume;
    }

    // 오디오 음소거/해제
    public void MuteSFX(bool mute)
    {
        audioSource.mute = mute;
    }
} 