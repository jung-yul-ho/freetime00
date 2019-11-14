using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyArm : MonoBehaviour
{
    public EnermyControl MyBody;

    private void OnTriggerEnter(Collider other)
    {
        if (MyBody.enermystate == EnermyState.ENERMY_ATTACK && other.transform.root.gameObject.tag == "Player")
        {
            Player.Instance.Damage(MyBody.Atk);
        }
    }
}
