using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip shoot, jump, land, crash, death;

    public void ShootSounds()
    {
        src.clip = shoot;
        src.Play();
    }

    public void JumpSounds()
    {
        src.clip = jump;
        src.Play();
    }
    public void JumpLand()
    {
        src.clip = land;
        src.Play();
    }

    public void CrashSound()
    {
        src.clip = crash;
        src.Play();
    }

    public void DeathSound()
    {
        src.clip = death;
        src.Play();
    }
}
