using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnermyHpBarControler : MonoBehaviour
{
    public Image BasicHpbarBack;
    public Image BasicHpbarFront;
    public EnermyControl enermy;
    Transform cam;
    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        BasicHpbarFront.rectTransform.sizeDelta = new Vector2((enermy.Hp / enermy.MaxHp) * 100.0f, 10.0f);
    }
}
