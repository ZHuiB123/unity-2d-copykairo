using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Build
{
    public int id;
    public double x;
    public double y;
    public int category;// 0 None 2 单人桌子 3 双人桌子 5 杂物
    public int serial;//0 1 2 3
    public int size;
    public bool isidle = true;

    public Build(int id,Vector3 pos_T,int category,int size,int serial)
    {
        this.id = id;
        this.x = pos_T.x;
        this.y = pos_T.y;
        this.category = category;
        this.size = size;
        this.serial = serial;
    }

    public Build()
    {
        this.id = 0;
        this.x = 0;
        this.y = 0;
        this.category = 0;
        this.size = 0;
        this.serial = 0;
    }

    public bool IdleChange()
    {
        isidle = !isidle;
        return isidle;
    }
}
