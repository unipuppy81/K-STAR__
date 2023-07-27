using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



// 캔버스 UI/UX 건드리는 스크립트
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


    // 줌할때 CrossHair Panel 키고 끄고
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
