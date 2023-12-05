using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RopeAction : MonoBehaviour
{
    public Transform player1;
    public GameObject player2;
    LineRenderer lr;
    SpringJoint sj;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Rope();
    }

    void Rope()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, player2.transform.position);

        sj = player1.gameObject.AddComponent<SpringJoint>();
        sj.autoConfigureConnectedAnchor = false;
        sj.connectedAnchor = player2.transform.position;

        float dis = Vector3.Distance(this.transform.position, player2.transform.position);

        sj.maxDistance = dis;
        sj.minDistance = dis * 0.5f;
    }
}
