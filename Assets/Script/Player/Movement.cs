using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CanvasManager canvas;

    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;

    private PhotonView pv;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private CinemachineVirtualCamera secondCamera; // ���� ī�޶�, ���� ī�޶� Ű : Z

    private GameObject cameraRoot; // ī�޶��� ��ġ
    private int cameraCount;

    [SerializeField]
    private GameObject bulletSpace;


    //  Z �ߺ� �Է� �ȵǰ� ������ �ִ� ����
    private float inputZTime;
    private float maxZtime;
    
    // ���� ī�޶� 1�� ī�޶�� �����Ǿ����� : 1�� ī�޶�(virtualCamera), 2�� ī�޶�(secondCamera) 
    public bool isMainCamera;
    

    void Start()
    {
        canvas = GameObject.Find("CanvasManager").GetComponent<CanvasManager>();

        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = controller.GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        secondCamera = GameObject.Find("PlayerFollowCamera2").GetComponent<CinemachineVirtualCamera>();

        maxZtime = 1.4f;

        isMainCamera = true;


        if (pv.IsMine)
        {
            cameraRoot = GameObject.FindWithTag("CameraRoot");
            virtualCamera.Follow = cameraRoot.transform;
            secondCamera.Follow = cameraRoot.transform;
        }
    }

    
    void Update()
    {
        CreateSometing();
        ChangeCamera();
    }

    void CreateSometing()
    {
        if (Input.GetKeyDown(KeyCode.Q) && pv.IsMine)
        {
            PhotonNetwork.Instantiate("Bullet", bulletSpace.transform.position, bulletSpace.transform.rotation, 0);
        }
    }

    void ChangeCamera()
    {
        inputZTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z) && inputZTime <= 0)
        {
            Debug.Log("Input Z");
            cameraCount++;
            isMainCamera = (cameraCount % 2 != 0 ? true : false);

            if (isMainCamera)
            {
                secondCamera.Priority = 12;
                canvas.isZoom = true;
            }
            else
            {
                secondCamera.Priority = 10;
                canvas.isZoom = false;
            }

            inputZTime = maxZtime;
        }
    }
}
