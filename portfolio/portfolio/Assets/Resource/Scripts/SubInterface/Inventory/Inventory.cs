using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //플레이어와 인벤토리 슬롯들을 연결하기위해 존재
    public List<InventroySlot> inventories;
    public bool InvenOpen;
    public Text Money;
    public Text ThrowOutText;

    static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Inventory>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        InvenOpen = true;
        for (int i = 0; i < 15; i++)
        {
            inventories[i].SlotNumber = i;
        }
        CloseInventory();
        SettingInven();
    }

    public void SettingInven()
    {
        for(int i = 0; i< 15; i++)
        {
            if(PlayerInformation.Instance.InvenList[i] != 0)
            {
                InItItem(PlayerInformation.Instance.InvenList[i]);
            }
        }
    }

    //아이템 만든뒤 배치시키기
    public bool InItItem(int itemnumber)
    {
        if (market.Instance.itemexplain.SelectItem != null)
        {
            if (market.Instance.itemexplain.SelectItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
            {
                for (int i = 0; i < 15; i++)
                {
                    if (inventories[i].InitItem != null)
                    {
                        if (inventories[i].InitItem.iteminformation.itemnumber == market.Instance.itemexplain.SelectItem.iteminformation.itemnumber)
                        {
                            inventories[i].InvetoySlotInit(itemnumber, true);
                            return true;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < 15; i++)
        {
            if (inventories[i].use == false)
            {
                inventories[i].InvetoySlotInit(itemnumber, true);
                return true;
            }
        }
        return false;
    }

    public void OpenInventroy()
    {
        if (InvenOpen == false)
        {
            transform.Translate(0, -1000, 0);
            InvenOpen = true;
            if(market.Instance.open == true)
            {
                ThrowOutText.text = "팔기";
            }
            else
            {
                ThrowOutText.text = "버리기";
            }
        }
    }

    public void CloseInventory()
    {
        if (InvenOpen == true)
        {
            transform.Translate(0, 1000, 0);
            InvenOpen = false;
        }
    }

    //대상인벤토리슬롯이 비어 있는지 확인
    public bool CheckForCanUsingThis(int i)
    {
        if(inventories[i].use == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void EquipItem()
    {
        if(SubInterfaceControler.Instance.ClickInventory != null)
        {
            if(SubInterfaceControler.Instance.ClickInventory.InitItem != null)
            {
                SubInterfaceControler.Instance.ItemEquipment(SubInterfaceControler.Instance.ClickInventory.InitItem);
            }
            else
            {
                MessageBox.Instance.ShowMessage("아이템이 없습니다");
            }
        }
        else
        {
            MessageBox.Instance.ShowMessage("인벤토리를 클릭해 주세요");
        }

    }

    public void UseItem()
    {
        if(SubInterfaceControler.Instance.ClickInventory == null)
        {
            MessageBox.Instance.ShowMessage("아무것도 없습니다");
        }
        else
        {
            SubInterfaceControler.Instance.ClickInventory.UsingItem();
        }

    }

    public void ThrowOutItme()
    {
        if(market.Instance.open == true)
        {
            SellItem();
        }
        else
        {
            SubInterfaceControler.Instance.ClickInventory.DestroyItem();
        }
    }

    public void SellItem()
    {
        market.Instance.Sell();
    }
}
