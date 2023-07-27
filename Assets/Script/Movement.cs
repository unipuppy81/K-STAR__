using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;

    private PhotonView pv;
    private CinemachineVirtualCamera virtualCamera;


    private GameObject obj;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = controller.GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        if (pv.IsMine)
        {
            obj = GameObject.FindWithTag("CameraRoot");
            virtualCamera.Follow = obj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)&& pv.IsMine) 
        {
            PhotonNetwork.Instantiate("DoorTest", transform.position, transform.rotation, 0);
        }
    }
}
