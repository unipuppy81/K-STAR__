using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    // 특정 위치로 순간이동할 대상 오브젝트
    public Transform teleportTarget;

    // 충돌이 발생했을 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("텔레포트");
            Debug.Log(teleportTarget.position);
            // "Player" 태그를 가진 오브젝트를 teleportTarget의 위치로 순간이동
            other.gameObject.transform.position = teleportTarget.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("텔레포트");
            Debug.Log(teleportTarget.position);
            collision.gameObject.transform.position = teleportTarget.position;
        }
    }
}
