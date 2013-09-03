using UnityEngine;
using System.Collections;

public class TakesDamage : MonoBehaviour 
{
    public int Health = 100;
    public bool IsAlive = true;
    public AudioClip DestroyAudio;

    private bool _audioPlaying;

    public void Update()
    {
        if (!IsAlive)
        {
            //if (!_audioPlaying)
            //{
            //    audio.PlayOneShot(DestroyAudio);
            //    _audioPlaying = true;
            //}
			AudioSource.PlayClipAtPoint(DestroyAudio, transform.position);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            IsAlive = false;
        }
    }
}
