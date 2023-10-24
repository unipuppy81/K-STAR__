using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviourPunCallbacks
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
        if (Input.GetMouseButtonDown(0) && pv.IsMine)
        {
            PhotonNetwork.Instantiate("SlotCar_A", Camera.main.transform.position, Camera.main.transform.rotation, 0);
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //닿은 곳의 정보
            RaycastHit hitInfo;
            //바라본다
            //Raycast는 기본적으로 bool형을 반환하게 된다. 그런데 인자 값에 out형으로 인자하나를 더 반환하게 된다.
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, 30.0f))
            {
                //닿았다 (Raycast가 true일때)
                if (hitInfo.transform.gameObject.tag == "Object")
                {
                    Destroy(hitInfo.transform.gameObject);
                }
            }
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
