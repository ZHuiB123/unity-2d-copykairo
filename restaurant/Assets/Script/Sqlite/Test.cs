using Mono.Data.Sqlite;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    SqliteConnection connection;
    SqliteCommand command;
    void Start()
    {
        Open();
        CreateTable();
        Insert();
        Select();
        Close();
    }

    void CreateTable()
    {
        command = new SqliteCommand("CREATE TABLE IF NOT EXISTS PERSON(ID INTEGER PRIMARY KEY AUTOINCREMENT,NAME TEXT,AGE INTEGER)", connection);
        command.ExecuteNonQuery();//执行
        command.Dispose();//结束
    }

    void Insert()
    {
        command = new SqliteCommand("INSERT INTO PERSON(NAME,AGE) VALUES('汤姆',40)", connection);
        command.ExecuteNonQuery();//执行
        command.Dispose();//结束
    }

    void Select()
    {
        command = new SqliteCommand("SELECT * FROM PERSON",connection);
        SqliteDataReader reader = command.ExecuteReader();
        while(reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            int age = reader.GetInt32(2);
            Debug.Log("id:" + id + " name:" + name + " age:" + age);
        }
    }

    void Open()
    {
        string path= Application.dataPath + "/SaveFiles" + "/sqlitedata.db";
        connection = new SqliteConnection("Data Source=" + path);
        connection.Open();
    }

    void Close()
    {
        connection.Close();
    }
}
