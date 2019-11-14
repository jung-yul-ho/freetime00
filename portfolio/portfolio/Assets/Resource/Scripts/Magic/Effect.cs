using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    EFFECT_NONE,
    EFFECT_AREA,
    EFFECT_ONE,
    EFFECT_MAGIC_SIRCLE
}

public enum MagicControler
{
    CONTROLER_PLAYER,
    CONTROLER_ENERMY
}

public class Effect : MonoBehaviour
{
    //마법형식
    public EffectType EffectType;
    //마법이름
    string EffectName;
    //마법이 유지되는 시간
    public float RemainTime;
    //마법의 위력
    public float EffectPower;
    //이펙트형식이 마법진일때 마법진 타임이 지나고 나면 해당넘버에 해당하는 마법이 발동되게한다
    public int summoneffect;
    //마법을 사용한 주체
    public MagicControler magiccontroler;
    //진짜이펙트 (충돌처리를위해 만듬)
    public Effect RealEffect;

    private void Update()
    {
        RemainTime -= 0.1f;
        if(RemainTime <= 0)
        {
            if(EffectType == EffectType.EFFECT_MAGIC_SIRCLE)
            {
                Effect neweffect = Instantiate<Effect>(GameEffectEngine.Instance.Skill[summoneffect]);
                neweffect.transform.position = transform.position;
                neweffect.magiccontroler = magiccontroler;
                neweffect.gameObject.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
