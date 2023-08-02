using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//레일카트에 탔을때 애니메이션 수행 스크립트(추후 애니메이션 추가시 작성)
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

        // 상호작용 대상의 상태에 따라 특정 동작 실행
        if (isOccupied)
        {
            // 탈 때 동작 실행 (예시: 특정 애니메이션 재생)
            // 플레이어가 오브젝트에 타고 있을 때 실행되는 동작을 구현
        }
        else
        {
            // 탈출 시 동작 실행 (예시: 특정 애니메이션 종료)
            // 플레이어가 오브젝트에서 탈출했을 때 실행되는 동작을 구현
        }
    }
}
