using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    // Ư�� ��ġ�� �����̵��� ��� ������Ʈ
    public Transform teleportTarget;

    // �浹�� �߻����� �� ȣ��Ǵ� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �±װ� "Player"���� Ȯ��
        if (other.CompareTag("Player"))
        {
            Debug.Log("�ڷ���Ʈ");
            Debug.Log(teleportTarget.position);
            // "Player" �±׸� ���� ������Ʈ�� teleportTarget�� ��ġ�� �����̵�
            other.gameObject.transform.position = teleportTarget.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("�ڷ���Ʈ");
            Debug.Log(teleportTarget.position);
            collision.gameObject.transform.position = teleportTarget.position;
        }
    }
}
