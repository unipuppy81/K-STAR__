using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class Rope3 : MonoBehaviour
{
    public bool startRope;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;

    public Transform[] startPoint; // ���� ���� ����
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
        //UpdateRope();
    }

    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.U)) { ClickObject(); }

        if(startRope) 
        { 
            // �ǽð����� ���� ������Ʈ
            UpdateRope();
        }
    }

    void ClickObject()
    {
        Collider[] colliders = new Collider[2];

        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, colliders, playerLayer);

        for (int i = 0; i < colliderCount; i++)
        {
            startPoint[i] = colliders[i].gameObject.transform;
        }

        startRope = true;
    }

    void UpdateRope()
    {
        float distance = Vector3.Distance(startPoint[0].position, startPoint[1].position);

        // �ִ� �Ÿ� ����
        if (distance > maxDistance)
        {
            endPoint.position = startPoint[0].position + (startPoint[1].position - startPoint[0].position).normalized * maxDistance;
        }

        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pos = Vector3.Lerp(startPoint[0].position, startPoint[1].position, t);
            lineRenderer.SetPosition(i, pos);
        }
    }
}
