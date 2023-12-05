using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope3 : MonoBehaviour
{
    public Transform startPoint; // 로프 시작 지점
    public Transform endPoint;   // 로프 끝 지점
    public int segments = 10;    // 로프 세그먼트 수
    public float maxDistance = 5f; // 로프의 최대 길이
    public float ropeWidth = 0.1f; // 로프 너비

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        UpdateRope();
    }

    void Update()
    {
        // 실시간으로 로프 업데이트
        UpdateRope();
    }

    void UpdateRope()
    {
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // 최대 거리 제한
        if (distance > maxDistance)
        {
            endPoint.position = startPoint.position + (endPoint.position - startPoint.position).normalized * maxDistance;
        }

        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pos = Vector3.Lerp(startPoint.position, endPoint.position, t);
            lineRenderer.SetPosition(i, pos);
        }
    }
}
