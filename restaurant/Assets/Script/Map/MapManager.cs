using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class MapManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Map;
    public GameObject[] AllBuild;

    private void Start()
    {
        Map = GameObject.FindGameObjectWithTag("Map").transform;
        LoadMapDate();
        LoadBuild();
    }

    public static void SaveThisGame()
    {
        SaveMapDate();
        print("存储成功");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))//存
        {
            SaveMapDate();
            print("存储成功");
        }
        if (Input.GetKeyDown(KeyCode.R))//读
        {
            LoadMapDate();
            LoadBuild();
            print("读取成功");
            //Debug.Log(MapMessage._instance.captical);
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            ReStartMapDate();
            print("重开成功");
        }
        //if (Input.GetKeyDown(KeyCode.C))//改
        //{
        //    MapMessage._instance.MapCategory_1[0] = 1;
        //    print("修改成功");
        //}
        if (Input.GetKeyDown(KeyCode.E))
        {
            for(int i=0;i<MapMessage._instance.BuildData.Count;i++)
            {
                print("(" + MapMessage._instance.BuildData[i].x + "," + MapMessage._instance.BuildData[i].y + ")" + MapMessage._instance.BuildData[i].isidle);
            }
        }
    }

    private static void ReStartMapDate()
    {
        MapDate date = new MapDate();
        string datapath = Application.dataPath + "/SaveFiles" + "/MapDatebyJson.json";
        string dateStr = JsonUtility.ToJson(date);
        StreamWriter sw = new StreamWriter(datapath); //创建一个写入流
        sw.Write(dateStr);//将dateStr写入
        sw.Close();//关闭流
    }

    private void LoadBuild()
    {
        //print(MapMessage._instance.BuildData.Count);
        //print(MapMessage._instance.BuildData[1]);
        for (int i = 0; i < MapMessage._instance.BuildData.Count; i++)
        {
            if (MapMessage._instance.BuildData[i].serial != 0) continue;
            Vector3 pos_T = new Vector3((float)MapMessage._instance.BuildData[i].x, (float)MapMessage._instance.BuildData[i].y, 0);
            //print(pos_T);
            GameObject newobj = Instantiate(AllBuild[MapMessage._instance.BuildData[i].id], pos_T, transform.rotation,Map);
            newobj.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }

    private static MapDate GetMapDate()
    {
        //MapDate date = new MapDate();
        MapMessage._instance.MapCategoryChangeToCate_1();
        MapDate date = new MapDate(MapMessage._instance.MapCategory_1,MapMessage._instance.captical,MapMessage._instance.BuildData,MapMessage._instance.BuildNum);
        return date;
    }

    private static void SetMapDate(MapDate date)
    {
        MapMessage._instance.MapCategory_1 = date.MapCategory;
        MapMessage._instance.captical = date.capital;
        MapMessage._instance.BuildData = date.BuildData;
        MapMessage._instance.BuildNum = date.BuildNum;
        MapMessage._instance.MapCategoryChange();
    }

    private static void SaveMapDate()
    {
        MapDate date = GetMapDate();
        string datapath = Application.dataPath + "/SaveFiles" + "/MapDatebyJson.json";
        string dateStr = JsonUtility.ToJson(date);
        StreamWriter sw = new StreamWriter(datapath); //创建一个写入流
        sw.Write(dateStr);//将dateStr写入
        sw.Close();//关闭流
    }

    private static void LoadMapDate()
    {
        string datePath = Application.dataPath + "/SaveFiles" + "/MapDatebyJson.json";
        if (File.Exists(datePath))  //判断这个路径里面是否为空
        {
            string jsonString = File.ReadAllText(Application.dataPath + "/SaveFiles" + "/MapDatebyJson.json");
            //MapDate date = JsonUtility.FromJson<MapDate>(jsonString);
            MapDate date = JsonMapper.ToObject<MapDate>(jsonString);//使用JsonMapper将遍历得到的jsonStr转换成Date对象
            SetMapDate(date);
        }
        else
        {
            Debug.Log("------未找到文件------");
        }
    }
}
