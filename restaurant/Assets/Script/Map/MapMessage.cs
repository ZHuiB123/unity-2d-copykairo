using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMessage : MonoBehaviour
{
    public static MapMessage _instance=null;

    public int[,] MapCategory = new int[14,11];//0 无 1 餐厅地板 2 单人餐桌 3 双人餐桌 4 厨房 5 杂物
    public int[] MapCategory_1 = new int[14*11];
    public int captical;
    public List<Build> BuildData = new List<Build>();// 0 None 2 单人桌子 3 双人桌子 5 杂物
    public List<int> BuildNum;


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this) { Destroy(gameObject); } //保证这个实例的唯一性
        //DontDestroyOnLoad(gameObject);
    }

    public void MapCategoryChange()
    {
        int z = 0;
        for(int i=0;i<14;i++)
        {
            for(int j=0;j<11;j++)
            {
                MapCategory[i, j] = MapCategory_1[z++];
            }
        }
    }

    public void MapCategoryChangeToCate_1()
    {
        int z = 0;
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                MapCategory_1[z++] = MapCategory[i, j];
            }
        }
    }
}
