using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour
{
    public Collider windZoneCollider; 

    private void OnColliderStay(Collider other)
    {
        // 윈드존의 영향 범위인지 확인
        //if (other == windZoneCollider)
        //{
            Rigidbody rb = other.attachedRigidbody;

            //if (rb != null)
            //{
                //바람 힘 얻음 
                Vector3 windForce = GetComponent<WindZone>().windMain * transform.forward;

                // 바람 힘을 오브젝트에 적용
                rb.AddForce(windForce, ForceMode.Force);
           // }
        //}
    }
}



