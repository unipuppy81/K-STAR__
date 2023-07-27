using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;

    private PhotonView pv;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private CinemachineVirtualCamera secondCamera;

    private GameObject obj;
    private int cameraCount;
    private bool isMainCamera;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = controller.GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        secondCamera = GameObject.Find("PlayerFollowCamera2").GetComponent<CinemachineVirtualCamera>();

        isMainCamera = true;


        if (pv.IsMine)
        {
            obj = GameObject.FindWithTag("CameraRoot");
            virtualCamera.Follow = obj.transform;
            secondCamera.Follow = obj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)&& pv.IsMine) 
        {
            PhotonNetwork.Instantiate("DoorTest", transform.position, transform.rotation, 0);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            cameraCount++;
            isMainCamera = (cameraCount % 2 != 0 ? true : false);

            if (isMainCamera)
            {
                secondCamera.Priority = 12;
            }
            else { 
                secondCamera.Priority = 10;
            }

        }
    }
}
