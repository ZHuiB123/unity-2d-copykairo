using Mono.Data.Sqlite;
using UnityEngine;

public class SqliteTableDouble : MonoBehaviour
{
    // Start is called before the first frame update
    SqliteConnection connection;
    SqliteCommand command;
    void Start()
    {
        Open();
        //DestoryTable();
        //CreateTable();
        //Insert();
        Select();
        Close();
    }

    // Update is called once per frame
    void CreateTable()
    {
        command = new SqliteCommand("CREATE TABLE IF NOT EXISTS TABLEDOUBLE(ID INTEGER PRIMARY KEY AUTOINCREMENT,NAME TEXT,CATEGORY INTEGER,SIZE INTEGER,PRICE INTEGER,LEVEL INTEGER,NUM INTEGER)", connection);
        //category 0 None 1 单人桌子 2 双人桌子 3 杂物
        command.ExecuteNonQuery();//执行
        command.Dispose();//结束
    }

    void DestoryTable()
    {
        command = new SqliteCommand("DROP TABLE TABLEDOUBLE", connection);
        command.ExecuteNonQuery();//执行
        command.Dispose();//结束
    }

    void Insert()
    {
        command = new SqliteCommand("UPDATE TABLEDOUBLE SET CATEGORY = 3 WHERE NAME = 'table_2'", connection);
        command.ExecuteNonQuery();//执行
        command.Dispose();//结束
        command = new SqliteCommand("UPDATE TABLEDOUBLE SET CATEGORY = 3 WHERE NAME = 'table_1'", connection);
        command.ExecuteNonQuery();//执行
        command.Dispose();//结束
        command = new SqliteCommand("UPDATE TABLEDOUBLE SET CATEGORY = 2 WHERE NAME = 'table_3'", connection);
        command.ExecuteNonQuery();//执行
        command.Dispose();//结束
        //command = new SqliteCommand("INSERT INTO TABLEDOUBLE(NAME,CATEGORY,SIZE,PRICE,LEVEL,NUM) VALUES('table_2',3,2,1500,2,0)", connection);
        //command.ExecuteNonQuery();//执行
        //command.Dispose();//结束
    }

    void Select()
    {
        command = new SqliteCommand("SELECT * FROM TABLEDOUBLE", connection);
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            int category = reader.GetInt32(2);
            int price = reader.GetInt32(4);
            int level = reader.GetInt32(5);
            int num = reader.GetInt32(6);
            Debug.Log("id:" + id + " name:" + name + " category:" + category + " price:" + price + " level:" + level + " num:" + num);
        }
    }

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
