using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    ////플레이어의 능력치현황
    //public int PlayerHp;
    //public int PlayerShield;
    //public float atk;
    //public float def;

    //스킬딜레이
    public float AttackDelay;
    public float ShieldDelay;
    public float FireFieldDelay;
    public float FireBallDelay;
    private float ContinueDamageDelay;

    public Item PreparePotion;

    static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Player>();
                }
            }
            return instance;
        }
    }

    public override void Damage(float Damage)
    {
        int def = PlayerInformation.Instance.Def;
        int shield = PlayerInformation.Instance.Shield;
        if (Damage > def)
        {
            if (shield > 0)
            {
                PlayerInformation.Instance.Shield -= (int)(Damage - def);
            }
            else
            {
                PlayerInformation.Instance.Hp -= (int)(Damage - def);
            }
        }
        else
        {
            if (shield > 0)
            {
                shield -= 1;
            }
            else
            {
                PlayerInformation.Instance.Hp -= 1;
            }
        }
    }

    private void Start()
    {
        if(PlayerInformation.Instance.ThisArea == PortalArea.PORTAL_VIALGE)
        {
            if (PlayerInformation.Instance.PastPortal == PortalArea.PORTAL_FIELDONE)
            {
                transform.Rotate(0, 180, 0);
            }
            else if (PlayerInformation.Instance.PastPortal == PortalArea.PORTAL_FIELDTWO)
            {
                transform.Rotate(0, 90, 0);
            }
        }
        else if (PlayerInformation.Instance.ThisArea == PortalArea.PORTAL_FIELDONE)
        {
            transform.Rotate(0, -90, 0);
        }
        else if(PlayerInformation.Instance.ThisArea == PortalArea.PORTAL_FIELDTWO)
        {
            transform.Rotate(0, -90, 0);
        }
        FollowCamMyVer.Instance.ResetCamera();
        JoystickControler.Instance.JoysticSetting();
    }

    void Update()
    {
        if (AttackDelay > 0)
        {
            //AttackDelay -= 0.02f;
            AttackDelay -= Time.deltaTime;
            if (AttackDelay <= 0)
            {
                PlayerControl.Instance.ResetAni();
            }
        }
        if (ShieldDelay > 0)
        {
            ShieldDelay -= 0.1f;
            if (ShieldDelay <= 0)
            {
                PlayerControl.Instance.ResetAni();
            }
        }
        if (FireFieldDelay > 0)
        {
            FireFieldDelay -= 0.1f;
            if (FireFieldDelay <= 0)
            {
                PlayerControl.Instance.ResetAni();
            }
        }
        if (FireBallDelay > 0)
        {
            FireBallDelay -= 0.1f;
            if (FireBallDelay <= 0)
            {
                PlayerControl.Instance.ResetAni();
            }
        }
        //지속데미지 관련 코드
        if (ContinueDamageDelay > 0)
        {
            ContinueDamageDelay -= 0.1f;
        }
    }

    //피격데미지(단일마법)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Magic")
        {
            if (other.gameObject.GetComponent<Effect>().EffectPower > PlayerInformation.Instance.Def && other.gameObject.GetComponent<Effect>().EffectType == EffectType.EFFECT_ONE)
            {
                if (PlayerInformation.Instance.Shield > 0)
                {
                    PlayerInformation.Instance.Shield -= (int)(other.gameObject.GetComponent<Effect>().EffectPower - PlayerInformation.Instance.Def);
                }
                else
                {
                    PlayerInformation.Instance.Hp -= (int)(other.gameObject.GetComponent<Effect>().EffectPower - PlayerInformation.Instance.Def);
                }
            }
        }
    }

    //지속데미지가 들어가는가?
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Magic")
        {
            if (other.gameObject.GetComponent<Effect>().EffectPower > PlayerInformation.Instance.Def && other.gameObject.GetComponent<Effect>().EffectType == EffectType.EFFECT_AREA)
            {
                if (ContinueDamageDelay <= 0)
                {
                    ContinueDamageDelay = 5.0f;
                    Damage(other.gameObject.GetComponent<Effect>().EffectPower);
                }
            }
        }
    }

    public void SetPreparePotion()
    {

    }
}
