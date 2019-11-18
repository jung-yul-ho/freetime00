using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;


public class SaveLoad : MonoBehaviour
{
    public List<int> ItemNumbers;
    public List<int> CounsmeCount;
    static SaveLoad instance;
    public static SaveLoad Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveLoad>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<SaveLoad>();
                }
            }
            return instance;
        }
    }

    //public void Start()
    //{
    //    Load();
    //}

    public void Save()
    {
        ItemNumbers = new List<int>();
        CounsmeCount = new List<int>();
        for (int i = 0; i < 15; i++)
        {
            if (Inventory.Instance.inventories[i].use == true)
            {
                ItemNumbers.Add(Inventory.Instance.inventories[i].InitItem.iteminformation.itemnumber);
                if(Inventory.Instance.inventories[i].InitItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
                {
                    CounsmeCount.Add(Inventory.Instance.inventories[i].InitItem.itemcount);
                }
            }
            else
            {
                ItemNumbers.Add(0);
            }
        }

        int head = 0;
        int body = 0;
        int weapon = 0;
        int consume = 0;
        int money = 0;

        if (Equipment.Instance.HeadSlot.use == true)
        {
            head = Equipment.Instance.HeadSlot.InitItem.iteminformation.itemnumber;
        }
        if (Equipment.Instance.BodySlot.use == true)
        {
            body = Equipment.Instance.BodySlot.InitItem.iteminformation.itemnumber;
        }
        if (Equipment.Instance.WeaponSlot.use == true)
        {
            weapon = Equipment.Instance.WeaponSlot.InitItem.iteminformation.itemnumber;
        }
        if(Equipment.Instance.ConsumeSlot.use == true)
        {
            consume = Equipment.Instance.ConsumeSlot.InitItem.iteminformation.itemnumber;
            CounsmeCount.Add(Equipment.Instance.ConsumeSlot.InitItem.itemcount);
        }
        money = PlayerInformation.Instance.Money;

        SaveData mysavedata = new SaveData(GameEingine.Instance.KillCount, ItemNumbers, head, body, weapon, consume, money, CounsmeCount);

        JsonData savedata = JsonMapper.ToJson(mysavedata);

        //저장경로
        File.WriteAllText(Application.persistentDataPath + "/11.json", savedata.ToString());

        ItemNumbers = null;
        CounsmeCount = null;
    }

    //플레이어의 인벤토리및 장비, 게임정보를 불러오는 코드
    public void Load()
    {

        if (File.Exists(Application.persistentDataPath + "/11.json"))
        {
            int InventoryNum = 0;
            int consumecount = 0;
            string mysavedata = File.ReadAllText(Application.persistentDataPath + "/11.json");
            JsonData myjsondata = JsonMapper.ToObject(mysavedata);
            GameEingine.Instance.KillCount = int.Parse(myjsondata["killcount"].ToString());
            for (int i = 0; i < 15; i++)
            {
                if (int.Parse(myjsondata["playeritem"][i].ToString()) != 0)
                {
                    Inventory.Instance.InItItem(int.Parse(myjsondata["playeritem"][i].ToString()));
                    if (Inventory.Instance.inventories[InventoryNum].InitItem.iteminformation.itemtype == ItemType.ITEM_CONSUME)
                    {
                        Inventory.Instance.inventories[InventoryNum].InitItem.itemcount = int.Parse(myjsondata["consumecount"][consumecount].ToString());
                        Inventory.Instance.inventories[InventoryNum].ItemCount.text = Inventory.Instance.inventories[InventoryNum].InitItem.itemcount.ToString();
                        consumecount++;
                    }
                    InventoryNum++;
                }
            }
            if (int.Parse(myjsondata["headnumber"].ToString()) != 0)
            {
                Item Newitem = Instantiate(ItemData.Instance.finditem(int.Parse(myjsondata["headnumber"].ToString())));
                Newitem.transform.parent = Equipment.Instance.HeadSlot.transform;
                Newitem.owner = Player.Instance;
                Newitem.InitInventoryslot = Equipment.Instance.HeadSlot;
                Equipment.Instance.HeadSlot.use = true;
                Equipment.Instance.HeadSlot.InitItem = Newitem;
                //이거 왜 이러지?
                Newitem.transform.localPosition = new Vector3(45, -45, 0);
            }
            if (int.Parse(myjsondata["bodynumber"].ToString()) != 0)
            {
                Item Newitem = Instantiate(ItemData.Instance.finditem(int.Parse(myjsondata["bodynumber"].ToString())));
                Newitem.transform.parent = Equipment.Instance.BodySlot.transform;
                Newitem.owner = Player.Instance;
                Newitem.InitInventoryslot = Equipment.Instance.BodySlot;
                Equipment.Instance.BodySlot.use = true;
                Equipment.Instance.BodySlot.InitItem = Newitem;
                //이거 왜 이러지?
                Newitem.transform.localPosition = new Vector3(45, -45, 0);
            }
            if (int.Parse(myjsondata["weaponnumber"].ToString()) != 0)
            {
                Item Newitem = Instantiate(ItemData.Instance.finditem(int.Parse(myjsondata["weaponnumber"].ToString())));
                Newitem.transform.parent = Equipment.Instance.WeaponSlot.transform;
                Newitem.owner = Player.Instance;
                Newitem.InitInventoryslot = Equipment.Instance.WeaponSlot;
                Equipment.Instance.WeaponSlot.use = true;
                Equipment.Instance.WeaponSlot.InitItem = Newitem;
                //이거 왜 이러지?
                Newitem.transform.localPosition = new Vector3(45, -45, 0);
            }
            if (int.Parse(myjsondata["consumenumber"].ToString()) != 0)
            {
                Item Newitem = Instantiate(ItemData.Instance.finditem(int.Parse(myjsondata["consumenumber"].ToString())));
                Newitem.transform.parent = Equipment.Instance.ConsumeSlot.transform;
                Newitem.owner = Player.Instance;
                Newitem.InitInventoryslot = Equipment.Instance.ConsumeSlot;
                Equipment.Instance.ConsumeSlot.use = true;
                Equipment.Instance.ConsumeSlot.InitItem = Newitem;
                //이거 왜 이러지?
                Newitem.transform.localPosition = new Vector3(45, -45, 0);
                Equipment.Instance.ConsumeSlot.InitItem.itemcount = int.Parse(myjsondata["consumecount"][consumecount].ToString());
                Equipment.Instance.ConsumeSlot.ItemCount.gameObject.SetActive(true);
                Equipment.Instance.ConsumeSlot.ItemCount.text = int.Parse(myjsondata["consumecount"][consumecount].ToString()).ToString();
            }
            PlayerInformation.Instance.Money = int.Parse(myjsondata["money"].ToString());
            PlayerInformation.Instance.SetMoney();
            PlayerInformation.Instance.CheckPlayerStat();
            Equipment.Instance.CheckForConsumeUi();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
