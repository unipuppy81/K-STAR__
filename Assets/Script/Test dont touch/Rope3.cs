using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope3 : MonoBehaviour
{
    public Transform startPoint; // ���� ���� ����
    public Transform endPoint;   // ���� �� ����
    public int segments = 10;    // ���� ���׸�Ʈ ��
    public float maxDistance = 5f; // ������ �ִ� ����
    public float ropeWidth = 0.1f; // ���� �ʺ�

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
        // �ǽð����� ���� ������Ʈ
        UpdateRope();
    }

    void UpdateRope()
    {
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // �ִ� �Ÿ� ����
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
