using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using Photon.Pun;

public class NewRope : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float maxRopeLength = 5f;

    private LineRenderer lineRenderer;
    public Transform[] startPoint; // ���� ���� ����


    public bool startRope;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;

    public bool startRope1;
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material.color = Color.gray;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        if (lineRenderer == null || player1 == null || player2 == null)
        {

            return;
        }

        InitializeRope();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) { ClickObject(); }

        if (startRope1)
        {
            UpdateRope();
        }
    }

    void InitializeRope()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint[0].position);
        lineRenderer.SetPosition(1, startPoint[1].position);
    }

    void UpdateRope()
    {
        if (startPoint.Length < 2)
        {
            return;
        }

        lineRenderer.SetPosition(0, startPoint[0].position);
        lineRenderer.SetPosition(1, startPoint[1].position);

        float currentRopeLength = Vector3.Distance(startPoint[0].position, startPoint[1].position);

        if (currentRopeLength > maxRopeLength)
        {
            // �ִ� ���� ���̸� �ʰ��ϸ� ������ ����ݴϴ�.
            Vector3 direction = (startPoint[1].position - startPoint[0].position).normalized;
            startPoint[1].position = startPoint[0].position + direction * maxRopeLength;
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

        startRope1 = true;
    }

}
