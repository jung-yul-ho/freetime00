using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalArea
{
    PORTAL_VIALGE,
    PORTAL_FIELDONE,
    PORTAL_FIELDTWO,
}

public class portal : MonoBehaviour
{
    public PortalArea PortalStarting;
    public PortalArea PortalTarget;
    private void OnTriggerEnter(Collider other)
    {
        TitleEngine.Instance.CharacterHp = PlayerInformation.Instance.Hp;
        TitleEngine.Instance.PastPortal = PortalStarting;
        SaveLoad.Instance.Save();
        if(PortalTarget == PortalArea.PORTAL_VIALGE)
        {
            if (other.gameObject.tag == "Player")
            {
                //플레이어 생성위치 세팅은 게임엔진에 존재 가능하다면 여기로 옮길것
                TitleEngine.Instance.StartVilageMap();
                //if (PortalStarting == PortalArea.PORTAL_FIELDONE)
                //{
                //    TitleEngine.Instance.StartVilageMap();
                //}
                //else if (PortalStarting == PortalArea.PORTAL_FIELDTWO)
                //{
                //    TitleEngine.Instance.StartVilageMap();
                //}
            }
        }
        else if(PortalTarget == PortalArea.PORTAL_FIELDONE)
        {
            if (other.gameObject.tag == "Player")
            {
                TitleEngine.Instance.StartBattleMap(1);
            }
        }
        else if(PortalTarget == PortalArea.PORTAL_FIELDTWO)
        {
            if (other.gameObject.tag == "Player")
            {
                TitleEngine.Instance.StartBattleMap(2);
            }
        }
    }
    
}
