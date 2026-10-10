using UnityEngine;


public class ZombieSounds : MonoBehaviour
{
    public AudioSource src;
    public AudioClip watdaRoar;

    public void Roar()
    {
        src.clip = watdaRoar;
        src.Play();
    }
}
