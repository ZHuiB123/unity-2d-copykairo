using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Architecture
{
    //ID INTEGER PRIMARY KEY AUTOINCREMENT,NAME TEXT,CATEGORY INTEGER,SIZE INTEGER,PRICE INTEGER,LEVEL INTEGER,NUM INTEGER
    public int id;
    public string name;
    public int category;
    public int size;
    public int price;
    public int level;
    public int num;

    public Architecture(int id,string name,int category,int size,int price,int level,int num)
    {
        this.id = id;
        this.category = category;
        this.size = size;
        this.price = price;
        this.level = level;
        this.num = num;
    }

    public Architecture()
    {
        return;
    }
}
