using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSound : MonoBehaviour
{
    private AudioSource audioSource;

    public Scrollbar SoundVolumeScrollbar;

    public AudioClip attackSound;
    public AudioClip damageSound;
    public AudioClip playerToObjectHitSound;
    public AudioClip playerDieSound;
    public AudioClip getBalloonSound;

    public AudioClip RiflerSkillSound;
    public AudioClip HunterSkillSound;

    public AudioClip ShielderSkillSound;
    public AudioClip MirrorlianSkillSound;

    public AudioClip FlashSkillSound;
    public AudioClip GhostSkillSound;

    public AudioClip VultureSkillSound;
    public AudioClip MagicianSkillSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //audioSource.volume 
    }

    public void ChangeVolume()
    {
        audioSource.volume = SoundVolumeScrollbar.value;
        SoundManager.Instance.ChangeVolume(SoundVolumeScrollbar.value);
        Debug.Log("Test " + audioSource.volume);
    }

    public void AttackSoundPlay()               { audioSource.PlayOneShot(attackSound); }
    public void DamageSoundPlay()               { audioSource.PlayOneShot(damageSound); }
    public void PlayerToObjectHitSoundPlay()    { audioSource.PlayOneShot(playerToObjectHitSound); }
    public void PlayerDieSoundPlay()            { audioSource.PlayOneShot(playerDieSound); }
    public void GetBalloonSoundPlay()           { audioSource.PlayOneShot(getBalloonSound); }

    public void RiflerSkillSoundPlay()          { audioSource.PlayOneShot(RiflerSkillSound); }
    public void HunterSkillSoundPlay()          { audioSource.PlayOneShot(HunterSkillSound); }

    public void ShielderSkillSoundPlay()        { audioSource.PlayOneShot(ShielderSkillSound); }
    public void MirrorlianSkillSoundPlay()      { audioSource.PlayOneShot(MirrorlianSkillSound); }

    public void FlashSkillSoundPlay()           { audioSource.PlayOneShot(FlashSkillSound); }
    public void GhostSkillSoundPlay()           { audioSource.PlayOneShot(GhostSkillSound); }

    public void VultureSkillSoundPlay()         { audioSource.PlayOneShot(VultureSkillSound); }
    public void MagicianSkillSoundPlay()        { audioSource.PlayOneShot(MagicianSkillSound); }
}
