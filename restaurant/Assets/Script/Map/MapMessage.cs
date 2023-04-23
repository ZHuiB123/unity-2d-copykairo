using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMessage : MonoBehaviour
{
    public static MapMessage _instance=null;

    public int[,] MapCategory = new int[14,11];//0 �� 1 �����ذ� 2 ���˲��� 3 ˫�˲��� 4 ���� 5 ����
    public int[] MapCategory_1 = new int[14*11];
    public int captical;
    public List<Build> BuildData = new List<Build>();// 0 None 2 �������� 3 ˫������ 5 ����
    public List<int> BuildNum;


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this) { Destroy(gameObject); } //��֤���ʵ����Ψһ��
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
