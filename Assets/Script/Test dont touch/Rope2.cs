using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope2 : MonoBehaviour
{
    public Transform player;          // 플레이어 오브젝트
    public Transform connectedObject;  // 연결할 오브젝트
    public float ropeWidth = 0.1f;    // 로프 두께
    public LayerMask obstacleLayer;   // 장애물을 감지할 레이어

    private GameObject ropeObject;    // 로프 GameObject

    void Update()
    {
        UpdateRope();
    }

    void UpdateRope()
    {
        // 플레이어와 연결된 오브젝트 간의 거리 계산
        float distance = Vector3.Distance(player.position, connectedObject.position);

        // 중간 지점 계산
        Vector3 midPoint = (player.position + connectedObject.position) / 2f;

        // 로프 생성 또는 업데이트
        if (ropeObject == null)
        {
            ropeObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ropeObject.name = "RopeCylinder";
        }

        ropeObject.transform.position = midPoint;
        ropeObject.transform.localScale = new Vector3(ropeWidth, distance / 2f, ropeWidth);

        // 플레이어와 연결된 오브젝트 간의 방향 벡터 계산
        Vector3 direction = (connectedObject.position - player.position).normalized;

        // 로프를 연결된 오브젝트의 방향으로 회전
        ropeObject.transform.up = direction;

        // 장애물 검사
        RaycastHit hit;
        if (Physics.Raycast(midPoint, direction, out hit, distance, obstacleLayer))
        {
            // 장애물이 있으면 장애물의 표면에 따라 로프를 회전
            Vector3 hitNormal = hit.normal;
            ropeObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
        }
    }
}
