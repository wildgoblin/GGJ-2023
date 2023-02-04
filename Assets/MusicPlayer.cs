using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    [SerializeField] float waitBeforePlaying;
    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip loop;

    AudioSource audioSource;


    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitThenPlay());
    }

    IEnumerator WaitThenPlay()
    {
        yield return new WaitForSeconds(waitBeforePlaying);
        audioSource.clip = intro;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = loop;
        audioSource.Play();
    }
}
