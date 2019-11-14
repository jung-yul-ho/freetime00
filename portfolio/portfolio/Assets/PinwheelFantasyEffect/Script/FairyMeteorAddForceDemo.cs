using UnityEngine;
using System.Collections;

public class FairyMeteorAddForceDemo : MonoBehaviour {

    public int strength;
    public Vector3 direction;

    protected Rigidbody rgbd;

    public void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        rgbd.AddForce(direction * strength);
    }

    //public bool pushOnAwake = true;
    //public Vector3 startDirection;
    //public float startMagnitude;
    //public ForceMode forceMode;

    //public GameObject head;
    //public GameObject tailone;
    //public GameObject tailtwo;
    //public GameObject dust;

    ////public int strength;
    ////public Vector3 direction;

    //protected Rigidbody rgbd;

    //public void Awake()
    //{
    //    rgbd = GetComponent<Rigidbody>();
    //}

    //public void Start()
    //{
    //    if (pushOnAwake)
    //    {
    //        Push(startDirection, startMagnitude);
    //    }
    //    //rgbd.AddForce(direction * strength);
    //}

    //public void Push(Vector3 direction, float magnitude)
    //{
    //    Vector3 dir = direction.normalized;
    //    rgbd.AddForce(dir * magnitude, forceMode);
    //}

    //public void StopParticleSystem(GameObject g)
    //{
    //    ParticleSystem[] par;
    //    par = g.GetComponentsInChildren<ParticleSystem>();
    //    foreach (ParticleSystem p in par)
    //    {
    //        p.Stop();
    //    }
    //}

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        rgbd.Sleep();
    //        if (head != null)
    //        {
    //            StopParticleSystem(head);
    //        }
    //        if (tailone != null)
    //        {
    //            StopParticleSystem(tailone);
    //        }
    //        if (tailtwo != null)
    //        {
    //            StopParticleSystem(tailtwo);
    //        }
    //        if (dust != null)
    //        {
    //            StopParticleSystem(dust);
    //        }
    //        //if (dust != null)
    //        //{
    //        //    dust.SetActive(true);
    //        //}
    //    }
    //}
}
