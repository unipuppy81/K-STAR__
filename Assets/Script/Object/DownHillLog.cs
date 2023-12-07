using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DownHillLog : MonoBehaviourPunCallbacks
{
    public GameObject objectToDropPrefab; // 떨어질 오브젝트 프리팹
    public float dropInterval = 2.0f; // 떨어뜨리는 간격
    public float hillWidth = 5.0f; // 언덕의 너비
    public float dropHeight = 5.0f; // 떨어뜨리는 높이
    public float spacing = 2.0f; // 오브젝트 간격

    private float nextDropTime = 1.0f;
    private float currentXPosition = -30.0f;

    void Update()
    {
        if (Time.time > nextDropTime)
        {
            DropObject();
            nextDropTime = Time.time + dropInterval;
        }
    }

    void DropObject()
    {
        // 일정 범위 내에서 떨어뜨리기
        Vector3 dropPosition = new Vector3(currentXPosition, transform.position.y + dropHeight, transform.position.z);

        // 포톤을 통해 동기화된 방식으로 오브젝트 생성
        PhotonNetwork.Instantiate(objectToDropPrefab.name, dropPosition, Quaternion.identity);

        // 다음 오브젝트의 위치 계산
        currentXPosition += spacing;
        if (Mathf.Abs(currentXPosition) > hillWidth / 2f)
        {
            // 너비를 넘어가면 다시 처음으로 돌아가기
            currentXPosition = -hillWidth / 2f;
        }
    }
}
