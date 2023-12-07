using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOnObject : MonoBehaviour
{
    public Transform targetObject; // 텍스트를 표시할 3D 오브젝트

    void Update()
    {
        // 3D 오브젝트의 월드 좌표를 스크린 좌표로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetObject.position);

        // 텍스트 오브젝트의 위치를 조정
        transform.position = new Vector3(screenPos.x, screenPos.y, 0f);
    }
}
