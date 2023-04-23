using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public static class SqliteArCategoryNum
{

    static SqliteConnection connection;
    static SqliteCommand command;

    public static int ArcCategoryNum()
    {
        Open();
        command = new SqliteCommand("SELECT COUNT(*) FROM TABLEDOUBLE", connection);
        SqliteDataReader reader = command.ExecuteReader();
        reader.Read();
        int num = reader.GetInt32(0);
        Close();
        return num;
    }

    static void Open()
    {
        string path = Application.dataPath + "/SaveFiles" + "/sqlitedata.db";
        connection = new SqliteConnection("Data Source=" + path);
        connection.Open();
    }

    static void Close()
    {
        connection.Close();
    }
}
