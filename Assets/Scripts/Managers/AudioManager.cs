using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClip;

    public void PlaySound(int i)
    {
        switch (audioClip[i].name)
        {
            case "button":
                audioSource.PlayOneShot(audioClip[i], 0.1f);
                break;

            case "card":
                audioSource.PlayOneShot(audioClip[i]);
                break;

            case "place":
                audioSource.PlayOneShot(audioClip[i], 1.5f);
                break;

            case "clash":
                audioSource.PlayOneShot(audioClip[i], 0.1f);
                break;
        }
    }
}
