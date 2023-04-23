using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class BuildObj : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Map;
    public GameObject[] Prompt;
    //public GameObject text;

    private GameObject build;
    private List<GameObject> GenPrompt = new List<GameObject>();
    private int[] pos_C = new int[2];
    private Architecture arc;
    private int[] savepos_c = new int[2];
    private int buildsize = 0;

    SqliteConnection connection;
    SqliteCommand command;

    public void SetObj(GameObject Obj)
    {
        build = Obj;
        arc = FindPriceOnSqlite(build.GetComponent<SpriteRenderer>().sprite.name);
        buildsize = arc.size;
    }

    void Awake()
    {
        Map = GameObject.FindGameObjectWithTag("Map").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //text.SetActive(true);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        pos_C = TransformCoordinate._instance.TransformToCoordinate(screenPos);
        Vector3 pos_T = Vector3.zero;
        if (pos_C!=savepos_c)
        {
            savepos_c = pos_C;
            if (GenPrompt.Count>0)
            for(int i=0;i<GenPrompt.Count;i++)
            {
                Destroy(GenPrompt[i]);
            }
            GenPrompt.Clear();
            pos_T = TransformCoordinate._instance.CoordinateToTransform(pos_C[0], pos_C[1]);
            transform.position = pos_T;
            PromptDisplay(pos_C,pos_T);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CameraMove._instance.enabled = true;
            ExitBuildPrompt._instance.ClosePrompt();
            if (GenPrompt.Count > 0)
                for (int i = 0; i < GenPrompt.Count; i++)
                {
                    Destroy(GenPrompt[i]);
                }
            GenPrompt.Clear();
            Destroy(gameObject);
        }
        if (Input.GetMouseButtonDown(0))
        {
            //pos_C = TransformCoordinate._instance.TransformToCoordinate(screenPos);
            //pos_T = TransformCoordinate._instance.CoordinateToTransform(pos_C[0], pos_C[1]);
            if(BuildArc(arc, pos_T, pos_C))
            {
                //text.SetActive(false);
                CameraMove._instance.enabled = true;
                ExitBuildPrompt._instance.ClosePrompt();
                if (GenPrompt.Count > 0)
                    for (int i = 0; i < GenPrompt.Count; i++)
                    {
                        Destroy(GenPrompt[i]);
                    }
                GenPrompt.Clear();
                Destroy(gameObject);
            }
        }
    }

    void PromptDisplay(int[] pos_C,Vector3 pos_T)
    {
        if(buildsize >= 1)
        {
            if(pos_C[0] < 0 || pos_C[0] > 13 || pos_C[1] < 0 || pos_C[1] > 10 || MapMessage._instance.MapCategory[pos_C[0], pos_C[1]] != 1)
            {
                GameObject newPrompt = Instantiate(Prompt[1], pos_T, transform.rotation);
                GenPrompt.Add(newPrompt);
            }
            else
            {
                GameObject newPrompt = Instantiate(Prompt[0], pos_T, transform.rotation);
                GenPrompt.Add(newPrompt);
            }
        }
        if(buildsize >= 2)
        {
            Vector3 PromptPosT = TransformCoordinate._instance.CoordinateToTransform(pos_C[0], pos_C[1] + 1);
            if (pos_C[0] < 0 || pos_C[0] > 13 || pos_C[1] < -1 || pos_C[1] > 9 || MapMessage._instance.MapCategory[pos_C[0], pos_C[1] + 1] != 1)
            {
                GameObject newPrompt = Instantiate(Prompt[1], PromptPosT, transform.rotation);
                GenPrompt.Add(newPrompt);
            }
            else
            {
                GameObject newPrompt = Instantiate(Prompt[0], PromptPosT, transform.rotation);
                GenPrompt.Add(newPrompt);
            }
        }
        if (arc.size == 4)
        {
            Vector3 PromptPosT1 = TransformCoordinate._instance.CoordinateToTransform(pos_C[0] + 1, pos_C[1] + 1);
            if (pos_C[0] < 0 || pos_C[0] > 13 || pos_C[1] < -1 || pos_C[1] > 9 || MapMessage._instance.MapCategory[pos_C[0] + 1, pos_C[1] + 1] != 1)
            {
                GameObject newPrompt = Instantiate(Prompt[1], PromptPosT1, transform.rotation);
                GenPrompt.Add(newPrompt);
            }
            else
            {
                GameObject newPrompt = Instantiate(Prompt[0], PromptPosT1, transform.rotation);
                GenPrompt.Add(newPrompt);
            }
            Vector3 PromptPosT2 = TransformCoordinate._instance.CoordinateToTransform(pos_C[0] + 1, pos_C[1]);
            if (pos_C[0] < 0 || pos_C[0] > 13 || pos_C[1] < -1 || pos_C[1] > 9 || MapMessage._instance.MapCategory[pos_C[0] + 1, pos_C[1]] != 1)
            {
                GameObject newPrompt = Instantiate(Prompt[1], PromptPosT2, transform.rotation);
                GenPrompt.Add(newPrompt);
            }
            else
            {
                GameObject newPrompt = Instantiate(Prompt[0], PromptPosT2, transform.rotation);
                GenPrompt.Add(newPrompt);
            }
        }
    }

    bool BuildArc(Architecture arc,Vector3 pos_T,int[] pos_C)
    {
        for (int i = 0; i < GenPrompt.Count; i++)
        {
            if(GenPrompt[i].name=="PromptCantBuild(Clone)")
            {
                Debug.Log("不可在此处建造！");
                return false;
            }
        }
        //if (pos_C[0]<0 || pos_C[0]>13 || pos_C[1]<0 || pos_C[1]>10 || MapMessage._instance.MapCategory[pos_C[0],pos_C[1]]!=1)
        //{
        //    Debug.Log("不可在此处建造！");
        //    return false;
        //}
        //if(arc.size >= 2)
        //{
        //    if (pos_C[1] >9 || MapMessage._instance.MapCategory[pos_C[0], pos_C[1] + 1] != 1)
        //    {
        //        Debug.Log("不可在此处建造！");
        //        return false;
        //    }
        //}
        //if (arc.size == 4)
        //{
        //    if (pos_C[0] > 12 || MapMessage._instance.MapCategory[pos_C[0] + 1, pos_C[1] + 1] != 1 || MapMessage._instance.MapCategory[pos_C[0] + 1, pos_C[1]] != 1)
        //    {
        //        Debug.Log("不可在此处建造！");
        //        return false;
        //    }
        //}
        if (MapMessage._instance.BuildNum[arc.id] > 0)
        {
            MapMessage._instance.BuildNum[arc.id]--;
            GenerateObj(pos_T);
            return true;
        }
        else if (MapMessage._instance.captical >= arc.price)
        {
            MapMessage._instance.captical -= arc.price;
            GenerateObj(pos_T);
            return true;
        }
        else
        {
            Debug.Log("资金不足");
            return true;
        }
    }

    void GenerateObj(Vector3 pos_T)
    {
        Instantiate(build, pos_T, transform.rotation, Map);
        ChangeMapCategory(pos_C, arc);
    }

    void ChangeMapCategory(int[] pos_C,Architecture arc)
    {
        if(arc.size>=1)
        {
            MapMessage._instance.MapCategory[pos_C[0], pos_C[1]] = arc.category;
            Vector3 pos_T = TransformCoordinate._instance.CoordinateToTransform(pos_C[0], pos_C[1]);
            Build GenerateBuild = new Build(arc.id, pos_T, arc.category, arc.size, 0);
            MapMessage._instance.BuildData.Add(GenerateBuild);
        }
        if(arc.size>=2)
        {
            MapMessage._instance.MapCategory[pos_C[0], pos_C[1] + 1] = arc.category;
            Vector3 pos_T = TransformCoordinate._instance.CoordinateToTransform(pos_C[0], pos_C[1] + 1);
            Build GenerateBuild = new Build(arc.id, pos_T, arc.category, arc.size, 1);
            MapMessage._instance.BuildData.Add(GenerateBuild);
        }
        if(arc.size>=4)
        {
            MapMessage._instance.MapCategory[pos_C[0] + 1, pos_C[1] + 1] = arc.category;
            Vector3 pos_T = TransformCoordinate._instance.CoordinateToTransform(pos_C[0] + 1, pos_C[1] + 1);
            Build GenerateBuild = new Build(arc.id, pos_T, arc.category, arc.size, 2);
            MapMessage._instance.BuildData.Add(GenerateBuild);
            MapMessage._instance.MapCategory[pos_C[0] + 1, pos_C[1]] = arc.category;
            pos_T = TransformCoordinate._instance.CoordinateToTransform(pos_C[0] + 1, pos_C[1]);
            GenerateBuild = new Build(arc.id, pos_T, arc.category, arc.size, 3);
            MapMessage._instance.BuildData.Add(GenerateBuild);
        }
    }

    Architecture FindPriceOnSqlite(string objname)
    {
        Open();
        command = new SqliteCommand("SELECT * FROM TABLEDOUBLE WHERE NAME='"+objname+"'", connection);
        SqliteDataReader reader = command.ExecuteReader();
        reader.Read();
        int id = reader.GetInt32(0);
        int category = reader.GetInt32(2);
        int size = reader.GetInt32(3);
        int price = reader.GetInt32(4);
        Close();
        Architecture arc = new Architecture();
        arc.id = id;
        arc.price = price;
        arc.size = size;
        arc.category = category;
        return arc;
    }

    //sqlite
    void Open()
    {
        string path = Application.dataPath + "/SaveFiles" + "/sqlitedata.db";
        connection = new SqliteConnection("Data Source=" + path);
        connection.Open();
    }

    void Close()
    {
        connection.Close();
    }

}
