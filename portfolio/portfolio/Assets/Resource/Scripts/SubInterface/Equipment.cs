using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public InventroySlot HeadSlot;
    public InventroySlot BodySlot;
    public InventroySlot WeaponSlot;
    public InventroySlot BookSlot;
    public InventroySlot ConsumeSlot;

    public Text Atk;
    public Text Def;

    public bool EquipOpen;

    static Equipment instance;
    public static Equipment Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Equipment>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Equipment>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        EquipOpen = true;
        CloseEquipment();
    }

    public void OpenEquipment()
    {
        if (EquipOpen == false)
        {
            transform.Translate(0, -1000, 0);
            EquipOpen = true;
        }
    }

    public void CloseEquipment()
    {
        if (EquipOpen == true)
        {
            transform.Translate(0, 1000, 0);
            EquipOpen = false;
        }
    }

    //장비한 아이템이 소비아이템일 경우 ui창과 연동
    public void CheckForConsumeUi()
    {
        if (ConsumeSlot.InitItem == null)
        {
            GameUiEingine.Instance.ConsumeCountTextUi.gameObject.SetActive(false);
            GameUiEingine.Instance.ConsumeSupport.sprite = GameUiEingine.Instance.DisablePostion;
        }
        else
        {
            GameUiEingine.Instance.ConsumeCountTextUi.gameObject.SetActive(true);
            GameUiEingine.Instance.ConsumeCountTextUi.text = ConsumeSlot.InitItem.itemcount.ToString();
            GameUiEingine.Instance.ConsumeSupport.sprite = ConsumeSlot.InitItem.iteminformation.itemsprite;
        }
    }

    public void ReleaseItem()
    {
        SubInterfaceControler.Instance.ClickInventory.InventorySlotRelease();
        CheckForConsumeUi();

        Player.Instance.CheckPlayerStat();
    }

    public void ReleaseAllEquipment()
    {
        HeadSlot.InventorySlotRelease();
        BodySlot.InventorySlotRelease();
        WeaponSlot.InventorySlotRelease();
        ConsumeSlot.InventorySlotRelease();
        Player.Instance.CheckPlayerStat();
    }

    //장비로인한 능력치 변동 표시
    public void EquipViewReset()
    {
        Atk.text = PlayerInformation.Instance.Atk.ToString();
        Def.text = PlayerInformation.Instance.Def.ToString();
    }
}
