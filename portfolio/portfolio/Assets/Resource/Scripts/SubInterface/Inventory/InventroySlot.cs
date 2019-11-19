using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum SLOTTYPE
{
    SLOTTYPE_BASIC,
    SLOTTYPE_TRASH,
    SLOTTYPE_EQUIPMENT,
    SLOTTYPE_SHOP,
}

public class InventroySlot : MonoBehaviour
{
    public SLOTTYPE slottype;
    public int SlotNumber;
    public bool use;
    public Item InitItem;
    public Text ItemCount;

    private void Awake()
    {
        use = false;

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
        entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
        entry_PointerEnter.callback.AddListener((data) => { PointerEnter((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerEnter);

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
        entry_PointerExit.eventID = EventTriggerType.PointerExit;
        entry_PointerExit.callback.AddListener((data) => { PointerExit((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerExit);

        EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { MouseDown((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerDown);

        EventTrigger.Entry entry_Drag = new EventTrigger.Entry();
        entry_Drag.eventID = EventTriggerType.Drag;
        entry_Drag.callback.AddListener((data) => { Drag((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_Drag);

        EventTrigger.Entry entry_EndDrag = new EventTrigger.Entry();
        entry_EndDrag.eventID = EventTriggerType.EndDrag;
        entry_EndDrag.callback.AddListener((data) => { EndDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_EndDrag);
    }

    void PointerEnter(PointerEventData data)
    {
        SubInterfaceControler.Instance.targetinvetory = this;
    }

    void PointerExit(PointerEventData data)
    {
        SubInterfaceControler.Instance.targetinvetory = null;
    }

    void MouseDown(PointerEventData data)
    {
        //if(market.Instance.open == false)
        //{
        //    if (use == true)
        //    {
        //        //if (InitItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
        //        //{
        //        //    UsingItem();
        //        //    //InitItem.UsingItem();
        //        //}
        //        //else if (InitItem.iteminformation.itemtype != ItemType.ITEM_CONSUME)
        //        //{

        //        //}
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}
        //else
        //{
        //    if(InitItem != null)
        //    {
        //        market.Instance.SelectItem(InitItem);
        //    }
        //}
        if(use == true)
        {
            SubInterfaceControler.Instance.ClickInventory = this;
            SubInterfaceControler.Instance.targetitem = InitItem;
            if (market.Instance.open == true)
            {
                market.Instance.SelectItem(InitItem);
            }
        }
        else if(market.Instance.open == true)
        {
            market.Instance.ResetExplain();
        }
        else
        {
            MessageBox.Instance.ShowMessage("아무것도 없습니다");
        }
    }

    void Drag(PointerEventData data)
    {
        if (market.Instance.open == false)
        {
            if (use == true && InitItem.iteminformation.itemtype != ItemType.ITEM_CONSUME)
            {
                InitItem.transform.position = Input.mousePosition;
                InitItem.transform.parent = SubInterfaceControler.Instance.transform;
            }
        }
        else
        {

        }
    }

    void EndDrag(PointerEventData data)
    {
        if(market.Instance.open == false)
        {        
            //마우스커서가 인벤토리 슬롯위에 있을때
            if (SubInterfaceControler.Instance.ClickInventory != null && SubInterfaceControler.Instance.targetinvetory != null)
            {
                //나자신에게 끌어다 놓을때
                if (SubInterfaceControler.Instance.targetinvetory == SubInterfaceControler.Instance.ClickInventory)
                {
                    SubInterfaceControler.Instance.ItemPositionMaintain();
                }
                //다른인벤토리창에 끌어다 놓을때
                else if (SubInterfaceControler.Instance.targetinvetory.slottype == SLOTTYPE.SLOTTYPE_BASIC)
                {
                    SubInterfaceControler.Instance.ItemPositionChange(SubInterfaceControler.Instance.ClickInventory, SubInterfaceControler.Instance.targetinvetory);
                }
                //쓰레기통에 끌어다 놓을때
                else if (SubInterfaceControler.Instance.targetinvetory.slottype == SLOTTYPE.SLOTTYPE_TRASH)
                {
                    DestroyItem();
                }
                //장비에다 끌어다 놓을때
                else if (SubInterfaceControler.Instance.targetinvetory.slottype == SLOTTYPE.SLOTTYPE_EQUIPMENT)
                {
                    SubInterfaceControler.Instance.ItemEquipment(InitItem);
                }
            }
            //마우스커서가 인벤토리 슬롯 위에 없을경우
            else if (SubInterfaceControler.Instance.ClickInventory != null)
            {
                SubInterfaceControler.Instance.ItemPositionMaintain();
            }
            PlayerInformation.Instance.CheckPlayerStat();
        }
    }

    //소비아이템을 사용했을때의 효과
    public void UsingItem()
    {
        if (InitItem != null && InitItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
        {
            InitItem.UsingItem();
            if (InitItem == null)
            {
                use = false;
                ItemCount.gameObject.SetActive(false);
            }
        }
    }

    //아이템삽입(아이템넘버, 아이템컨트롤러)
    public void InvetoySlotInit(int itemnumber, bool ControlerPlayer)
    {
        if(use == true)
        {
            if(InitItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
            {
                InitItem.itemcount++;
                ConsumeItemSetting();
            }
        }
        else
        {
            Item Newitem = Instantiate(ItemData.Instance.finditem(itemnumber));
            Newitem.transform.parent = transform;
            Newitem.InitInventoryslot = this;
            use = true;
            InitItem = Newitem;
            //if (ControlerPlayer == true)
            //{
            //    Newitem.owner = Player.Instance;
            //}
            //이거 왜 이러지?
            Newitem.transform.localPosition = new Vector3(45, -45, 0);
            //소모품일경우 하나가 존재한다는 의미부여
            if(Newitem.iteminformation.itemtype == ItemType.ITEM_CONSUME && slottype == SLOTTYPE.SLOTTYPE_BASIC)
            {
                Newitem.itemcount = 1;
                ItemCount.gameObject.SetActive(true);
            }
        }
    }
    
    //장비아이템해제
    public void InventorySlotRelease()
    {
        if(use == true)
        {
            bool pass = false;
            if (slottype == SLOTTYPE.SLOTTYPE_EQUIPMENT)
            {
                for (int i = 0; i < 15; i++)
                {
                    if (Inventory.Instance.CheckForCanUsingThis(i) == true)
                    {
                        SubInterfaceControler.Instance.ItemPositionChange(this, Inventory.Instance.inventories[i]);
                        pass = true;
                        break;
                    }
                }
            }
            if (pass == false)
            {
                MessageBox.Instance.ShowMessage("인벤토리칸이 가득 찼습니다.");
            }
        }
    }

    public void ConsumeItemSetting()
    {
        ItemCount.gameObject.SetActive(true);
        ItemCount.text = InitItem.itemcount.ToString();
        if(InitItem.itemcount == 0)
        {
            ItemCount.gameObject.SetActive(false);
        }
    }

    public void RealeaseInventory()
    {
        if(slottype != SLOTTYPE.SLOTTYPE_BASIC)
        {
            for(int i = 0; i< 15; i++)
            {
                if(Inventory.Instance.CheckForCanUsingThis(i) == true)
                {
                    ItemMove(Inventory.Instance.inventories[i]);
                    break;
                }
            }
        }
    }

    public void ItemMove(InventroySlot inventoryslot)
    {
        //타겟 아이템슬롯이 비어있을경우 (그냥 이동)
        if (inventoryslot.use == false)
        {
            inventoryslot.InitItem = InitItem;
            InitItem = null;
            inventoryslot.InitItem.transform.parent = inventoryslot.transform;
            inventoryslot.InitItem.transform.localPosition = new Vector3(45, -45, 0);
            use = false;
            inventoryslot.use = true;
        }
    }

    public void DestroyItem()
    {
        Destroy(SubInterfaceControler.Instance.targetitem.gameObject);
        use = false;
    }
}