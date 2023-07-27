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
    private CinemachineVirtualCamera secondCamera; // 줌인 카메라, 줌인 카메라 키 : Z

    private GameObject cameraRoot; // 카메라의 위치
    private int cameraCount;

    [SerializeField]
    private GameObject bulletSpace;


    //  Z 중복 입력 안되게 딜레이 주는 변수
    private float inputZTime;
    private float maxZtime;
    
    // 메인 카메라가 1번 카메라로 설정되었는지 : 1번 카메라(virtualCamera), 2번 카메라(secondCamera) 
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
