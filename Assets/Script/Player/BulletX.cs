using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//���� �� ������ �Ѿ� �ڱ� ���忡�� ���� �Ѿ� �ڱ��� ����ġ�� ���´�.
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
           Debug.Log("�ѹ߻�");
           transform.Translate(Vector3.forward * 0.1f);
    }
}


