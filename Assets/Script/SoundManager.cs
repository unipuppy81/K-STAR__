using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // AudioSource가 없을 경우 오류 출력
           
            return;
        }

        // Inspector에서 AudioClip이 할당되었는지 확인
        if (audioClip == null)
        {
            
        }
        else
        {
            // AudioClip이 할당되었으면 AudioSource에 연결
            audioSource.clip = audioClip;
        }


        PlaySound();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
 
        }
    }

    void PlaySound()
    {
        Debug.Log("A");
        audioSource.Play();
    }
}
