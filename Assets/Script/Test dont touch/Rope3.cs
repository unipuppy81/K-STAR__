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

    public Transform[] startPoint; // 로프 시작 지점
    public int segments = 10;    // 로프 세그먼트 수
    public float maxDistance = 5f; // 로프의 최대 길이
    public float ropeWidth = 0.1f; // 로프 너비

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
            // 실시간으로 로프 업데이트
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

        // startPoint[0]의 움직임에 따라 startPoint[1]이 따라오도록 설정
        startPoint[1].position = startPoint[0].position + direction * currentDistance;

        // startPoint[1]의 움직임에 따라 startPoint[0]이 따라오도록 설정
        startPoint[0].position = startPoint[1].position - direction * currentDistance;

        // 최대 거리 제한
        if (currentDistance > maxDistance)
        {
            // 최대 거리에 도달하면 두 지점을 고정
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

        // 최대 거리 제한
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
