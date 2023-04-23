using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapDate
{

    //[SerializeField]
    public int capital;
    public int[] MapCategory;//0 �� 1 �����ذ� 2 ���˲��� 3 ˫�˲��� 4 ���� 5 ����
    public List<Build> BuildData;// 0 None 1 �������� 2 ˫������ 3 ���� 4 ˫������(��λ��) 5 �������� 6 ��������(��λ��) 7 ��������(��λ��) 8 ��������(��λ��)
    public List<int> BuildNum;

    public MapDate(int[] MapCategory,int captical, List<Build> BuildData, List<int> BuildNum) 
    {
        this.MapCategory = MapCategory;
        this.capital = captical;
        this.BuildData = BuildData;
        this.BuildNum = BuildNum;
    }
    public MapDate()
    {
        MapCategory = new int[14 * 11]
        {
            1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 ,//0
            1,1,1,1,1,4,4,4,4,0,0 ,//1
            1,1,1,1,1,4,4,4,4,0,0 ,//2
            1,1,1,1,1,4,4,4,4,0,0 ,//3
            1,1,1,1,1,4,4,4,4,0,0 ,//4
            1,1,1,1,1,4,4,4,4,0,0 ,//5
            1,1,1,1,1,4,4,4,4,0,0 ,//6
            1,1,1,1,1,1,1,1,1,1,1 ,//7
            1,1,1,1,1,1,1,1,1,1,1 ,//8
            1,1,1,1,1,1,1,1,1,1,1 ,//9
            1,1,1,1,1,1,1,1,1,1,1 ,//10
            1,1,1,1,1,1,1,1,1,1,1 ,//11
            1,1,1,1,1,1,1,1,1,1,1 ,//12
            1,1,1,1,1,1,1,1,1,1,1 ,//13
        };
        capital = 15000;
        BuildData = new List<Build>();
        int[] Buildnum = new int[SqliteArCategoryNum.ArcCategoryNum() + 1];
        BuildNum = new List<int>(Buildnum);
    }
}

