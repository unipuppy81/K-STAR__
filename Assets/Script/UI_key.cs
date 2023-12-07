using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_key : MonoBehaviour
{

    public GameObject text;

    private void Start()
    {
        text.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�ؽ�Ʈ ǥ��");
            text.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
