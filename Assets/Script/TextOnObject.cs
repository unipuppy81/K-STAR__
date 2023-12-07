using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOnObject : MonoBehaviour
{
    public Transform targetObject; // �ؽ�Ʈ�� ǥ���� 3D ������Ʈ

    void Update()
    {
        // 3D ������Ʈ�� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetObject.position);

        // �ؽ�Ʈ ������Ʈ�� ��ġ�� ����
        transform.position = new Vector3(screenPos.x, screenPos.y, 0f);
    }
}
