using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour
{
    //! 두 오브젝트에 리지드바디 주고 mass에 맞게 기울며
    //! 최대 기울기 값을 주어 그 이상 넘어 가지 않도록 한다 .
    //기획서에서 양탄지가 기울어 질때 플레이어가 낙사시 게임이 오버
    // 위 부분에 대해 다시 물어볼것 (양탄지가 90도 이상 기울어 지며 플레이어를 떨어 트릴것인지, 기울기에 값을 주어 위에서 버티게 할것인지 )
    public float maxTiltAngle = 10f; // 최대 기울기 각도

    private float initialTiltAngle;
    private bool playerOnObject;
    private Rigidbody playerRigidbody;
    private Rigidbody objectRigidbody;

    private void Start()
    {
        // 초기 오브젝트의 기울기를 저장합니다.
        initialTiltAngle = transform.rotation.eulerAngles.x;
    }

    private void Update()
    {
        if (playerOnObject && playerRigidbody != null && objectRigidbody != null)
        {
            // 플레이어의 질량을 기준으로 기울기를 계산합니다.
            float tiltAngle = Mathf.Lerp(initialTiltAngle, maxTiltAngle, playerRigidbody.mass / objectRigidbody.mass);

            // 오브젝트를 기울입니다.
            Vector3 tiltDirection = playerRigidbody.transform.position - transform.position;
            Vector3 tiltAxis = Vector3.Cross(transform.up, tiltDirection.normalized);
            transform.rotation = Quaternion.AngleAxis(tiltAngle, tiltAxis) * Quaternion.Euler(initialTiltAngle, 0f, 0f);
            //!뭔말인지 모르겠음 다시 보도
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerOnObject = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnObject = false;
            playerRigidbody = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }
}
