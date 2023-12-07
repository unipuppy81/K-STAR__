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
            // AudioSource�� ���� ��� ���� ���
           
            return;
        }

        // Inspector���� AudioClip�� �Ҵ�Ǿ����� Ȯ��
        if (audioClip == null)
        {
            
        }
        else
        {
            // AudioClip�� �Ҵ�Ǿ����� AudioSource�� ����
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
