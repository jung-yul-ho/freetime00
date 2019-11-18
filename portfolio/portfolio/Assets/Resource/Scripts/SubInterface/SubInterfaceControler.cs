using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using LitJson;

public enum SubInterfaceState
{
    SUB_NONE,
    SUB_EQUIPMENT,
    SUB_INVENTORY,
    SUB_MARKET
}

public class SubInterfaceControler : MonoBehaviour
{
    public InventroySlot ClickInventory;
    public InventroySlot targetinvetory;
    public Item targetitem;

    //인벤토리와 메뉴의 실질적 오브젝트들
    public GameObject Menu;

    //서브인터페이스의 상태
    SubInterfaceState subinterfacestate;

    static SubInterfaceControler instance;
    public static SubInterfaceControler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SubInterfaceControler>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<SubInterfaceControler>();
                }
            }
            return instance;
        }
    }

    public void OpenMenu()
    {
        if (Menu.activeSelf == false)
        {
            transform.Translate(-480, 0, 0);
            Menu.SetActive(true);
        }
        else
        {
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        if (Menu.activeSelf == true)
        {
            transform.Translate(480, 0, 0);
            Menu.SetActive(false);
        }
    }

    //장비및인벤토리 리셋(완전파괴)
    public void ResetAllSubInterface()
    {
        for (int i = 0; i < 10; i++)
        {
            Inventory.Instance.inventories[i].use = false;
            Destroy(Inventory.Instance.inventories[i].InitItem);
        }
        Equipment.Instance.HeadSlot.use = false;
        Destroy(Equipment.Instance.HeadSlot.InitItem);
        Equipment.Instance.BodySlot.use = false;
        Destroy(Equipment.Instance.BodySlot.InitItem);
        Equipment.Instance.WeaponSlot.use = false;
        Destroy(Equipment.Instance.WeaponSlot.InitItem);
    }

    //아이템 위치 유지
    public void ItemPositionMaintain()
    {
        if(targetitem != null)
        {
            ClickInventory.InitItem.transform.parent = ClickInventory.transform;
            ClickInventory.InitItem.transform.localPosition = new Vector3(45, -45, 0);
            ClickInventory = null;
        }
    }

    //아이템 위치 변경
    public void ItemPositionChange(InventroySlot PastInventory, InventroySlot targetInventory)
    {
        //타겟 아이템슬롯이 비어있을경우 (그냥 이동)
        if (targetInventory.InitItem == null)
        {
            targetInventory.InitItem = PastInventory.InitItem;
            if (targetInventory.InitItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
            {
                PastInventory.ItemCount.gameObject.SetActive(false);
                targetInventory.ItemCount.gameObject.SetActive(true);
                targetInventory.ItemCount.text = targetInventory.InitItem.itemcount.ToString();
            }
            PastInventory.InitItem = null;
            targetInventory.InitItem.transform.parent = targetInventory.transform;
            targetInventory.InitItem.transform.localPosition = new Vector3(40, -40, 0);
            PastInventory.use = false;
            targetInventory.use = true;
            PastInventory = null;
            targetInventory.InitItem.InitInventoryslot = targetinvetory;
        }
        //내 아이템이 장비슬롯에 있지만 대상이 장비슬롯과 호환되지 않을때
        else if (PastInventory == Equipment.Instance.HeadSlot || PastInventory == Equipment.Instance.WeaponSlot || PastInventory == Equipment.Instance.BodySlot)
        {
            if (targetInventory.InitItem.iteminformation.itemtype == PastInventory.InitItem.iteminformation.itemtype)
            {
                Item emptyitem = targetInventory.InitItem;
                targetInventory.InitItem = PastInventory.InitItem;
                targetInventory.InitItem.transform.parent = targetInventory.transform;
                targetInventory.InitItem.transform.localPosition = new Vector3(45, -45, 0);
                PastInventory.InitItem = emptyitem;
                PastInventory.InitItem.transform.parent = PastInventory.transform;
                PastInventory.InitItem.transform.localPosition = new Vector3(45, -45, 0);
                if (targetInventory.InitItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
                {
                    PastInventory.ItemCount.gameObject.SetActive(false);
                    targetInventory.ItemCount.gameObject.SetActive(true);
                    targetInventory.ItemCount.text = targetInventory.InitItem.itemcount.ToString();
                }
            }
            else
            {
                ItemPositionMaintain();
            }
            targetInventory.InitItem.InitInventoryslot = targetinvetory;
            PastInventory.InitItem.InitInventoryslot = PastInventory;
        }
        //타겟 아이템슬롯이 존재할 경우 (아이템 위치 서로 전환)
        else
        {
            Item emptyitem = targetInventory.InitItem;
            targetInventory.InitItem = PastInventory.InitItem;
            targetInventory.InitItem.transform.parent = targetInventory.transform;
            targetInventory.InitItem.transform.localPosition = new Vector3(45, -45, 0);
            PastInventory.InitItem = emptyitem;
            PastInventory.InitItem.transform.parent = PastInventory.transform;
            PastInventory.InitItem.transform.localPosition = new Vector3(45, -45, 0);
            if (targetInventory.InitItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
            {
                targetInventory.ItemCount.gameObject.SetActive(true);
                targetInventory.ItemCount.text = targetInventory.InitItem.itemcount.ToString();
            }
            else
            {
                targetInventory.ItemCount.gameObject.SetActive(false);
            }
            if (PastInventory.InitItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
            {
                PastInventory.ItemCount.gameObject.SetActive(true);
                PastInventory.ItemCount.text = PastInventory.InitItem.itemcount.ToString();
            }
            else
            {
                PastInventory.ItemCount.gameObject.SetActive(false);
            }
            targetInventory.InitItem.InitInventoryslot = targetinvetory;
            PastInventory.InitItem.InitInventoryslot = PastInventory;
        }
    }

    //아이템장비
    public void ItemEquipment(Item item)
    {
        if (item.iteminformation.itemtype == ItemType.ITEM_HEAD)
        {
            if(Equipment.Instance.HeadSlot.use == true)
            {
                ItemExchange(ItemType.ITEM_HEAD);
            }
            else
            {
                Equipment.Instance.HeadSlot.use = true;
                ClickInventory.use = false;
                targetitem.transform.parent = Equipment.Instance.HeadSlot.transform;
                targetitem.transform.localPosition = new Vector3(45, -45, 0);
                Equipment.Instance.HeadSlot.InitItem = targetitem;
                ClickInventory.InitItem = null;
                ClickInventory = null;
                targetitem.InitInventoryslot = Equipment.Instance.HeadSlot;
            }
        }
        else if (targetitem.iteminformation.itemtype == ItemType.ITEM_BODY)
        {
            if (Equipment.Instance.BodySlot.use == true)
            {
                ItemExchange(ItemType.ITEM_BODY);
            }
            else
            {
                Equipment.Instance.BodySlot.use = true;
                ClickInventory.use = false;
                targetitem.transform.parent = Equipment.Instance.BodySlot.transform;
                targetitem.transform.localPosition = new Vector3(45, -45, 0);
                Equipment.Instance.BodySlot.InitItem = targetitem;
                ClickInventory.InitItem = null;
                ClickInventory = null;
                targetitem.InitInventoryslot = Equipment.Instance.BodySlot;
            }
        }
        else if (targetitem.iteminformation.itemtype == ItemType.ITEM_WEAPON)
        {
            if (Equipment.Instance.WeaponSlot.use == true)
            {
                ItemExchange(ItemType.ITEM_WEAPON);
            }
            else
            {
                Equipment.Instance.WeaponSlot.use = true;
                ClickInventory.use = false;
                targetitem.transform.parent = Equipment.Instance.WeaponSlot.transform;
                targetitem.transform.localPosition = new Vector3(45, -45, 0);
                Equipment.Instance.WeaponSlot.InitItem = targetitem;
                ClickInventory.InitItem = null;
                ClickInventory = null;
                targetitem.InitInventoryslot = Equipment.Instance.WeaponSlot;
            }
        }
        else if(targetitem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
        {
            if (Equipment.Instance.ConsumeSlot.use == true)
            {
                if(Equipment.Instance.ConsumeSlot.InitItem.iteminformation.itemnumber == targetitem.iteminformation.itemnumber)
                {
                    Equipment.Instance.ConsumeSlot.InitItem.itemcount += targetitem.itemcount;
                    Equipment.Instance.ConsumeSlot.ItemCount.text = Equipment.Instance.ConsumeSlot.InitItem.itemcount.ToString();
                    Destroy(targetitem.gameObject);
                    ClickInventory.use = false;
                    ClickInventory.ItemCount.gameObject.SetActive(false);
                    ClickInventory.InitItem = null;
                    ClickInventory = null;
                    Equipment.Instance.CheckForConsumeUi();
                }
                else
                {
                    ItemExchange(ItemType.ITEM_CONSUME);
                }
            }
            else
            {
                Equipment.Instance.ConsumeSlot.use = true;
                ClickInventory.use = false;
                targetitem.transform.parent = Equipment.Instance.ConsumeSlot.transform;
                targetitem.transform.localPosition = new Vector3(45, -45, 0);
                Equipment.Instance.ConsumeSlot.InitItem = targetitem;
                ClickInventory.ItemCount.gameObject.SetActive(false);
                ClickInventory.InitItem = null;
                ClickInventory = null;
                Equipment.Instance.ConsumeSlot.ItemCount.gameObject.SetActive(true);
                Equipment.Instance.ConsumeSlot.ItemCount.text = Equipment.Instance.ConsumeSlot.InitItem.itemcount.ToString();
                Equipment.Instance.CheckForConsumeUi();
                targetitem.InitInventoryslot = Equipment.Instance.ConsumeSlot;
            }

        }
        PlayerInformation.Instance.CheckPlayerStat();
    }

    //장비한 아이템 교체
    void ItemExchange(ItemType itemtype)
    {
        if(itemtype == ItemType.ITEM_HEAD)
        {
            Item emptyitem = Equipment.Instance.HeadSlot.InitItem;
            Equipment.Instance.HeadSlot.InitItem = targetitem;
            Equipment.Instance.HeadSlot.InitItem.transform.parent = Equipment.Instance.HeadSlot.transform;
            Equipment.Instance.HeadSlot.InitItem.transform.localPosition = new Vector3(45, -45, 0);
            ClickInventory.InitItem = emptyitem;
            ClickInventory.InitItem.transform.parent = ClickInventory.transform;
            ClickInventory.InitItem.transform.localPosition = new Vector3(45, -45, 0);
        }
        else if(itemtype == ItemType.ITEM_BODY)
        {
            Item emptyitem = Equipment.Instance.BodySlot.InitItem;
            Equipment.Instance.BodySlot.InitItem = targetitem;
            Equipment.Instance.BodySlot.InitItem.transform.parent = Equipment.Instance.BodySlot.transform;
            Equipment.Instance.BodySlot.InitItem.transform.localPosition = new Vector3(45, -45, 0);
            ClickInventory.InitItem = emptyitem;
            ClickInventory.InitItem.transform.parent = ClickInventory.transform;
            ClickInventory.InitItem.transform.localPosition = new Vector3(45, -45, 0);
        }
        else if(itemtype == ItemType.ITEM_WEAPON)
        {
            Item emptyitem = Equipment.Instance.WeaponSlot.InitItem;
            Equipment.Instance.WeaponSlot.InitItem = targetitem;
            Equipment.Instance.WeaponSlot.InitItem.transform.parent = Equipment.Instance.WeaponSlot.transform;
            Equipment.Instance.WeaponSlot.InitItem.transform.localPosition = new Vector3(45, -45, 0);
            ClickInventory.InitItem = emptyitem;
            ClickInventory.InitItem.transform.parent = ClickInventory.transform;
            ClickInventory.InitItem.transform.localPosition = new Vector3(45, -45, 0);
        }
        else if(itemtype == ItemType.ITEM_CONSUME)
        {
            Item emptyitem = Equipment.Instance.ConsumeSlot.InitItem;
            Equipment.Instance.ConsumeSlot.InitItem = targetitem;
            Equipment.Instance.ConsumeSlot.InitItem.transform.parent = Equipment.Instance.ConsumeSlot.transform;
            Equipment.Instance.ConsumeSlot.InitItem.transform.localPosition = new Vector3(45, -45, 0);
            ClickInventory.InitItem = emptyitem;
            ClickInventory.InitItem.transform.parent = ClickInventory.transform;
            ClickInventory.InitItem.transform.localPosition = new Vector3(45, -45, 0);
        }
    }

    //가진돈 재확인
    public void MoneySetting()
    {
        Inventory.Instance.Money.text = PlayerInformation.Instance.Money.ToString();
    }
}
