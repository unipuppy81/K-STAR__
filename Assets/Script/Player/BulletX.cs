using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//총을 쏜 지점에 총알 자국 공장에서 만든 총알 자국을 그위치에 놓는다.
public class BulletX : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFire();
    }

    private void UpdateFire()
    {
           Debug.Log("총발사");
           transform.Translate(Vector3.forward * 0.1f);
    }
}


