using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControler : MonoBehaviour
{
    public GameObject Player;
    public int Time = 0;

    void FixedUpdate()
    {
        Time++;
        transform.Translate(transform.up, Space.World);
        if(Time >= 200)
        {
            Destroy(gameObject);
        }
    }
}
