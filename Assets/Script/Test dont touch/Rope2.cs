using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope2 : MonoBehaviour
{
    public Transform player;          // �÷��̾� ������Ʈ
    public Transform connectedObject;  // ������ ������Ʈ
    public float ropeWidth = 0.1f;    // ���� �β�
    public LayerMask obstacleLayer;   // ��ֹ��� ������ ���̾�

    private GameObject ropeObject;    // ���� GameObject

    void Update()
    {
        UpdateRope();
    }

    void UpdateRope()
    {
        // �÷��̾�� ����� ������Ʈ ���� �Ÿ� ���
        float distance = Vector3.Distance(player.position, connectedObject.position);

        // �߰� ���� ���
        Vector3 midPoint = (player.position + connectedObject.position) / 2f;

        // ���� ���� �Ǵ� ������Ʈ
        if (ropeObject == null)
        {
            ropeObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ropeObject.name = "RopeCylinder";
        }

        ropeObject.transform.position = midPoint;
        ropeObject.transform.localScale = new Vector3(ropeWidth, distance / 2f, ropeWidth);

        // �÷��̾�� ����� ������Ʈ ���� ���� ���� ���
        Vector3 direction = (connectedObject.position - player.position).normalized;

        // ������ ����� ������Ʈ�� �������� ȸ��
        ropeObject.transform.up = direction;

        // ��ֹ� �˻�
        RaycastHit hit;
        if (Physics.Raycast(midPoint, direction, out hit, distance, obstacleLayer))
        {
            // ��ֹ��� ������ ��ֹ��� ǥ�鿡 ���� ������ ȸ��
            Vector3 hitNormal = hit.normal;
            ropeObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
        }
    }
}
