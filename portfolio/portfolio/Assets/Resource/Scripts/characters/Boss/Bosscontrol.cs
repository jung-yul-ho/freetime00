using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bosscontrol : MonoBehaviour
{
    //보스 사망 체크
    public bool die;
    //보스 기본 오브젝트
    public GameObject charObj;
    //보스 사망시 레그돌 오브젝트
    public GameObject ragdollObj;
    //보스 리지드바디 레그돌 보조
    public Rigidbody spine;


    static Bosscontrol instance;

    public static Bosscontrol Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Bosscontrol>();
                if (instance == null)
                {
                    GameObject container = new GameObject("보스없음");
                    instance = container.AddComponent<Bosscontrol>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        die = false;
    }

    void Update()
    {

    }

    public void BossDamage()
    {

    }

    public void BossDie()
    {
        CopyAnimCharacterTransformToRagdoll(charObj.transform, ragdollObj.transform);
        ragdollObj.transform.position = charObj.transform.position;
        ragdollObj.transform.rotation = charObj.transform.rotation;
        charObj.gameObject.SetActive(false);
        ragdollObj.gameObject.SetActive(true);
        spine.AddForce(new Vector3(0f, 0f, 5000f));
        //die = true;
    }

    private void CopyAnimCharacterTransformToRagdoll(Transform origin, Transform rag)
    {
        for (int i = 0; i < origin.transform.childCount; i++)
        {
            if (origin.transform.childCount != 0)
            {
                CopyAnimCharacterTransformToRagdoll(origin.transform.GetChild(i), rag.transform.GetChild(i));
            }
            rag.transform.GetChild(i).localPosition = origin.transform.GetChild(i).localPosition;
            rag.transform.GetChild(i).localRotation = origin.transform.GetChild(i).localRotation;
        }
    }

    public void BossAppear()
    {
        charObj.SetActive(true);
    }
}
