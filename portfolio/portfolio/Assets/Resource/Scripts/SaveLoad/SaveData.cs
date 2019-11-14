using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class SaveData
{
    public int killcount;
    public List<int> playeritem;
    public int headnumber;
    public int bodynumber;
    public int weaponnumber;
    public int consumenumber;
    public int money;
    public List<int> consumecount;

    public SaveData(int killcount, List<int> items, int head, int body, int weapon,int consume , int money, List<int> ConsumeCount)
    {
        this.killcount = killcount;
        this.playeritem = items;
        this.headnumber = head;
        this.bodynumber = body;
        this.weaponnumber = weapon;
        this.consumenumber = consume;
        this.money = money;
        this.consumecount = ConsumeCount;
    }

    public SaveData()
    {

    }

    //public int killcount;

    //public SaveData(int killcount)
    //{
    //    this.killcount = 1111;
    //    //killcount = this.killcount;
    //}
}
