using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
    }

    public void BulletMove()
    {
        transform.Translate(Vector3.forward * 0.1f);
    }
}
