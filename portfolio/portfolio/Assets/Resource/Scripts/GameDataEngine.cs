using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataEngine : MonoBehaviour
{
    public TextAsset ItemText;
    public TextAsset EnermyText;
    public TextAsset SkillText;

    static GameDataEngine instance;
    public static GameDataEngine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameDataEngine>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<GameDataEngine>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        LoadItemData();
        LoadSkillData();
    }

    //private void Start()
    //{
    //    LoadItemData();
    //}
    //시작하자마자 불러옴
    //아이템정보 불러오기
    void LoadItemData()
    {
        List<Dictionary<string, object>> data = csvreader.Read(ItemText);
        foreach (var el in data)
        {
            int a = (int)el["Itemnumber"];
            Item newitem = Instantiate<Item>(ItemData.Instance.Itemprefab);
            newitem.iteminformation.itemnumber = a;
            newitem.iteminformation.itemname = (string)el["ItemName"];
            newitem.iteminformation.ItemCost = (int)el["ItemCost"];
            newitem.iteminformation.itemexplain = (string)el["ItemExplain"];
            newitem.atk = (int)el["ItemAtk"];
            newitem.def = (int)el["ItemDef"];
            newitem.itempower = (int)el["ItemPower"];
            newitem.name = (string)el["ItemName"];
            ItemData.Instance.find[a] = newitem;
            newitem.iteminformation.itemsprite = ItemData.Instance.ItemImage[a - 1];
            newitem.GetComponent<Image>().sprite = newitem.iteminformation.itemsprite;
            if ((int)el["ItemType"] == 0)
            {
                newitem.iteminformation.itemtype = ItemType.ITEM_CONSUME;
                newitem.transform.parent = Item_Consume.Instance.transform;
                Item_Consume.Instance.items.Add(newitem);
            }
            else if((int)el["ItemType"] == 1)
            {
                newitem.iteminformation.itemtype = ItemType.ITEM_BODY;
                newitem.transform.parent = Item_Body.Instance.transform;
                Item_Body.Instance.items.Add(newitem);
            }
            else if ((int)el["ItemType"] == 2)
            {
                newitem.iteminformation.itemtype = ItemType.ITEM_WEAPON;
                newitem.transform.parent = Item_Weapon.Instance.transform;
                Item_Weapon.Instance.items.Add(newitem);
            }
            else if ((int)el["ItemType"] == 3)
            {
                newitem.iteminformation.itemtype = ItemType.ITEM_HEAD;
                newitem.transform.parent = Item_Head.Instance.transform;
                Item_Head.Instance.items.Add(newitem);
            }
            else if((int)el["ItemType"] == 4)
            {
                newitem.iteminformation.itemtype = ItemType.ITEM_BOOK;
                newitem.transform.parent = Item_Book.Instance.transform;
                Item_Book.Instance.items.Add(newitem);
            }
        }
    }

    public void LoadSkillData()
    {
        List<Dictionary<string, object>> data = csvreader.Read(SkillText);

        foreach (var el in data)
        {
            int a = (int)el["SkillNumber"];
            GameEffectEngine.Instance.Skill[a].RemainTime = (int)el["SkillTime"];
            GameEffectEngine.Instance.Skill[a].EffectPower = (int)el["SkillPower"];
            GameEffectEngine.Instance.Skill[a].EffectType = (EffectType)el["SkillType"];
            GameEffectEngine.Instance.Skill[a].summoneffect = (int)el["SummonEffect"];
            if (GameEffectEngine.Instance.Skill[a].RealEffect != null)
            {
                GameEffectEngine.Instance.Skill[a].RealEffect.RemainTime = (int)el["SkillTime"];
                GameEffectEngine.Instance.Skill[a].RealEffect.EffectPower = (int)el["SkillPower"];
                GameEffectEngine.Instance.Skill[a].RealEffect.EffectType = (EffectType)el["SkillType"];
                GameEffectEngine.Instance.Skill[a].RealEffect.summoneffect = (int)el["SummonEffect"];
            }
        }
    }
    
    //적을 생성할때 사용함
    //적정보 불러오기
    public void LoadEnermyData(GameObject obj)
    {
        List<Dictionary<string, object>> data = csvreader.Read(EnermyText);
        foreach (var el in data)
        {
            var target = obj.GetComponent<EnermyControl>();
            if(obj.GetComponent<EnermyControl>().EnermyNumber == (int)el["EnermyNumber"])
            {
                target.Enermyname = (string)el["EnermyName"];
                target.Atk = (int)el["EnermyAtk"];
                target.Def = (int)el["EnermyDef"];
                target.MaxHp = (int)el["EnermyHp"];
                target.Hp = (int)el["EnermyHp"];
                break;
            }
        }
    }

    public void LoadBossData(GameObject obj)
    {
        List<Dictionary<string, object>> data = csvreader.Read(EnermyText);
        foreach (var el in data)
        {
            var target = obj.GetComponent<BossActivity>();
            if (obj.GetComponent<BossActivity>().BossNumber == (int)el["EnermyNumber"])
            {
                target.BossName = (string)el["EnermyName"];
                target.Atk = (int)el["EnermyAtk"];
                target.Def = (int)el["EnermyDef"];
                target.BossMaxHp = (int)el["EnermyHp"];
                target.Hp = (int)el["EnermyHp"];
                break;
            }
        }
    }
}