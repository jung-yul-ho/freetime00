using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class market : MonoBehaviour
{
    public bool open;
    public GameObject RealMarket;
    public ItemViewer itemexplain;

    public List<InventroySlot> MarketInventory;
    static market instance;
    public static market Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<market>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<market>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        open = true;
        BasicMarketSetting();
        MarketClose();
    }

    public void MarketOpen()
    {
        if(open==false)
        {
            RealMarket.SetActive(true);
            open = true;
            Inventory.Instance.OpenInventroy();
        }
    }

    public void MarketClose()
    {
        if(open == true)
        {
            RealMarket.SetActive(false);
            open = false;
            Inventory.Instance.CloseInventory();
        }
    }

    //아이템을 선택했을경우 설명창에 띄운다
    public void SelectItem(Item item)
    {
        itemexplain.ItemImage.sprite = item.iteminformation.itemsprite;
        itemexplain.SelectItem = item;
        itemexplain.ItemExplain.text = item.iteminformation.itemexplain.ToString();
        itemexplain.ItemMoney.text = item.iteminformation.ItemCost.ToString() + "G";
    }

    //설명창을 초기화 시킨다
    public void ResetExplain()
    {
        itemexplain.ItemImage.sprite = null;
        itemexplain.SelectItem = null;
        itemexplain.ItemExplain.text = "아이템이 선택되지 않았습니다.";
        itemexplain.ItemMoney.text = "0" + "G";
    }

    public void Buy()
    {
        if (PlayerInformation.Instance.Money >= itemexplain.SelectItem.iteminformation.ItemCost)
        {
            if(Inventory.Instance.InItItem(itemexplain.SelectItem.iteminformation.itemnumber) == true)
            {
                PlayerInformation.Instance.Money -= itemexplain.SelectItem.iteminformation.ItemCost;
                SubInterfaceControler.Instance.MoneySetting();
            }
        }
        else if(PlayerInformation.Instance.Money < itemexplain.SelectItem.iteminformation.ItemCost)
        {
            MessageBox.Instance.ShowMessage("돈이 부족합니다");
        }
    }

    public void Sell()
    {
        if(itemexplain.SelectItem != null)
        {
            PlayerInformation.Instance.Money += itemexplain.SelectItem.iteminformation.ItemCost;
            SubInterfaceControler.Instance.MoneySetting();
            if (itemexplain.SelectItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
            {
                itemexplain.SelectItem.itemcount--;
                itemexplain.SelectItem.InitInventoryslot.ConsumeItemSetting();
                if (itemexplain.SelectItem.itemcount <= 0)
                {
                    itemexplain.SelectItem.InitInventoryslot.use = false;
                    itemexplain.SelectItem.InitInventoryslot.InitItem = null;
                    itemexplain.ItemImage.sprite = null;
                    Destroy(itemexplain.SelectItem.gameObject);
                    itemexplain.SelectItem = null;
                    itemexplain.ItemExplain.text = "";
                    itemexplain.ItemMoney.text = "0" + "G";
                }
            }
            else
            {
                itemexplain.SelectItem.InitInventoryslot.use = false;
                itemexplain.SelectItem.InitInventoryslot.InitItem = null;
                itemexplain.ItemImage.sprite = null;
                Destroy(itemexplain.SelectItem.gameObject);
                itemexplain.SelectItem = null;
                itemexplain.ItemExplain.text = "";
                itemexplain.ItemMoney.text = "0" + "G";
            }
        }
        else
        {
            MessageBox.Instance.ShowMessage("판매할 아이템이 없습니다");
        }
    }

    //기초 시장 세팅
    public void BasicMarketSetting()
    {
        MarketInventory[0].InvetoySlotInit(1, false);
        MarketInventory[1].InvetoySlotInit(3, false);
        MarketInventory[2].InvetoySlotInit(6, false);
        MarketInventory[3].InvetoySlotInit(8, false);
    }
}