using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource     audioSource;

    public AudioClip        MenuMusic;
    public AudioClip        InGameMusic;
    public AudioClip        ButtonClickSound;

    public AudioClip        GameOverSound;
    public AudioClip        PlayerDieSound;

    //public Auti

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeVolume(float value)
    {
        audioSource.volume = value;
    }

    public void MenuSoundPlay()
    {
        audioSource.clip = MenuMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void InGameSoundPlay()
    {
        audioSource.clip = InGameMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SoundStop()
    {
        audioSource.Stop();
    }



    public void ButtonClickPlay()
    {
        audioSource.PlayOneShot(ButtonClickSound);
    }

    public void GameOverSoundPlay()
    {
        audioSource.PlayOneShot(GameOverSound);
    }

    public void PlayerDieSoundPlay()
    {
        audioSource.PlayOneShot(PlayerDieSound);
    }
}
