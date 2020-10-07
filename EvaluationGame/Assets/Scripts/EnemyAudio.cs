using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] AudioClip _hurtNoise;
    private AudioSource _myAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        _myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHurtSound()
    {
        _myAudioSource.PlayOneShot(_hurtNoise);
    }
}
