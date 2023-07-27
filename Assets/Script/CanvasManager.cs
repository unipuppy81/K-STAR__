using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



// ĵ���� UI/UX �ǵ帮�� ��ũ��Ʈ
public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject CrosshairPanel;

    public bool isZoom;

    void Start()
    {
        isZoom = false;
    }

    void Update()
    {
        StartCoroutine(SetCrossHair());
    }


    // ���Ҷ� CrossHair Panel Ű�� ����
    IEnumerator SetCrossHair()
    {
        if (isZoom)
        {
            yield return new WaitForSeconds(1.0f);
            CrosshairPanel.SetActive(true);
 
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            CrosshairPanel.SetActive(false);
        }
    }

}
