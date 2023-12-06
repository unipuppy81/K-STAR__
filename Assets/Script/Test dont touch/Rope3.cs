using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting.InputSystem;
using UnityEngine;


public class Rope3 : MonoBehaviour
{
    public bool startRope;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;

    public Transform[] startPoint; // ���� ���� ����
    public int segments = 10;    // ���� ���׸�Ʈ ��
    public float maxDistance = 5f; // ������ �ִ� ����
    public float ropeWidth = 0.1f; // ���� �ʺ�

    private LineRenderer lineRenderer;


    public bool isMaxLength;


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
            if (isMaxLength)
            {
                MaxLength();
            }
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
        Vector3 direction = (startPoint[1].position - startPoint[0].position).normalized;
        float currentDistance = Vector3.Distance(startPoint[0].position, startPoint[1].position);

        // startPoint[0]�� �����ӿ� ���� startPoint[1]�� ��������� ����
        startPoint[1].position = startPoint[0].position + direction * currentDistance;

        // startPoint[1]�� �����ӿ� ���� startPoint[0]�� ��������� ����
        startPoint[0].position = startPoint[1].position - direction * currentDistance;

        // �ִ� �Ÿ� ����
        if (currentDistance > maxDistance)
        {
            // �ִ� �Ÿ��� �����ϸ� �� ������ ����
            startPoint[0].position = startPoint[1].position - direction * maxDistance;
            startPoint[1].position = startPoint[0].position + direction * maxDistance;

            isMaxLength = true;
        }

        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pos = Vector3.Lerp(startPoint[0].position, startPoint[1].position, t);
            lineRenderer.SetPosition(i, pos);
        }
    }

    void MaxLength()
    {
        foreach(var a in startPoint)
        {
            a.gameObject.GetComponent<ThirdPersonController>().isMaxLength = true;
        }
    }
    /*
    void UpdateRope()
    {
        float distance = Vector3.Distance(startPoint[0].position, startPoint[1].position);

        // �ִ� �Ÿ� ����
        if (distance > maxDistance)
        {
            startPoint[1].position = startPoint[0].position + (startPoint[1].position - startPoint[0].position).normalized * maxDistance;
            startPoint[0].GetComponent<Rigidbody>().isKinematic = true;
            startPoint[1].GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            startPoint[0].GetComponent<Rigidbody>().isKinematic = false;
            startPoint[1].GetComponent<Rigidbody>().isKinematic = false;
        }



        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pos = Vector3.Lerp(startPoint[0].position, startPoint[1].position, t);
            lineRenderer.SetPosition(i, pos);
        }
    }
    */
}
