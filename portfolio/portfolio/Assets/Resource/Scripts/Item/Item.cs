using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    public ItemInformation iteminformation;
    public RectTransform rect;
    public InventroySlot InitInventoryslot;
    public int atk;
    public int def;
    public int itempower;
    public int itemcount;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void UsingItem()
    {
        if (iteminformation.itemtype == ItemType.ITEM_NULL)
        {

        }
        else if (iteminformation.itemtype == ItemType.ITEM_CONSUME)
        {
            if(PlayerInformation.Instance.Hp < 100)
            {
                if (itemcount > 1)
                {
                    itemcount--;
                    PlayerInformation.Instance.Hp += itempower;
                    if(PlayerInformation.Instance.Hp > 100)
                    {
                        PlayerInformation.Instance.Hp = 100;
                    }
                    InitInventoryslot.ConsumeItemSetting();
                }
                else
                {
                    PlayerInformation.Instance.Hp += itempower;
                    if (PlayerInformation.Instance.Hp > 100)
                    {
                        PlayerInformation.Instance.Hp = 100;
                    }
                    InitInventoryslot.ConsumeItemSetting();
                    InitInventoryslot.InitItem = null;
                    InitInventoryslot.use = false;
                    InitInventoryslot.ItemCount.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            else
            {
                MessageBox.Instance.ShowMessage("HP가 가득찼습니다");
            }
        }
    }

    public ItemType ReturnItemType()
    {
        return iteminformation.itemtype;
    }
}
