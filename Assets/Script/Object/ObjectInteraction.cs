using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//����īƮ�� ������ �ִϸ��̼� ���� ��ũ��Ʈ(���� �ִϸ��̼� �߰��� �ۼ�)
public class ObjectInteraction :MonoBehaviourPunCallbacks
{
    private bool isOccupied = false;

    public bool IsOccupied()
    {
        return isOccupied;
    }

    [PunRPC]
    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;

        // ��ȣ�ۿ� ����� ���¿� ���� Ư�� ���� ����
        if (isOccupied)
        {
            // Ż �� ���� ���� (����: Ư�� �ִϸ��̼� ���)
            // �÷��̾ ������Ʈ�� Ÿ�� ���� �� ����Ǵ� ������ ����
        }
        else
        {
            // Ż�� �� ���� ���� (����: Ư�� �ִϸ��̼� ����)
            // �÷��̾ ������Ʈ���� Ż������ �� ����Ǵ� ������ ����
        }
    }
}
