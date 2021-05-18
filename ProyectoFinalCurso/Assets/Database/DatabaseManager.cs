using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System;

public class DatabaseManager : MonoBehaviour
{

    public string levelName;
    public Text checkForDuplicatesTXT;
    string connectionString;

    public int zOffset;

    public List<String> existingTables;

    public GameObject sideA;
    public GameObject sideB;

    public EditorAsset[] sideAObjects;
    public EditorAsset[] sideBObjects;

    public GameObject[] prefabs;

    void Start()
    {
        connectionString = Application.dataPath + "/Database/FlipBoard.db";
        //SpawnObjectsFromDatabase();
        ListExistingTables();
    }

    public void SaveLevel()
    {
        if (levelName != "" && checkForDuplicatesTXT.text == "OK")
        {
            CreateLevelTable();

            sideAObjects = sideA.GetComponentsInChildren<EditorAsset>();
            sideBObjects = sideB.GetComponentsInChildren<EditorAsset>();

            for (int i = 0; i < sideAObjects.Length; i++)
            {
                AddToDataTable(sideAObjects[i].gridPosY, sideAObjects[i].gridPosX, Convert.ToInt32(sideAObjects[i].twoP), Convert.ToInt32(sideAObjects[i].sideA), (int)sideAObjects[i].size, (int)sideAObjects[i].asset);
            }
            for (int i = 0; i < sideBObjects.Length; i++)
            {
                AddToDataTable(sideBObjects[i].gridPosY, sideBObjects[i].gridPosX, Convert.ToInt32(sideBObjects[i].twoP), Convert.ToInt32(sideBObjects[i].sideA), (int)sideBObjects[i].size, (int)sideBObjects[i].asset);
            }
            Debug.Log("GUARDADO");
        }
        else
        {
            Debug.Log("NO SE PUEDE GUARDAR");
        }
    }

    void AddToDataTable(int posY, int posX, int twoP, int sideA, int size, int prefabId)
    {
        string conn = "URI=file:" + connectionString; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO " + levelName + " (PosY, PosX, TwoP, SideA, Size, PrefabID) VALUES (" + posY + "," + posX + "," + twoP + "," + sideA + "," + size + "," + prefabId + ")";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    void CreateLevelTable()
    {
        string conn = "URI=file:" + connectionString; //Path to database.
        IDbConnection dbconn;
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "CREATE TABLE " + levelName + " (PropID INTEGER NOT NULL, PosY INTEGER NOT NULL," +
            "PosX INTEGER NOT NULL, TwoP INTEGER NOT NULL, SideA INTEGER NOT NULL, Size INTEGER NOT NULL," +
            "PrefabID INTEGER NOT NULL, PRIMARY KEY(PropID AUTOINCREMENT))";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    
    void SpawnObjectsFromDatabase()
    {
        string conn = "URI=file:" + connectionString; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT PosY, PosX, TwoP, SideA, Size, PrefabID " + "FROM "+ levelName + " WHERE 1";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {

            int posY = reader.GetInt32(0);
            int posX = reader.GetInt32(1);
            int twoP = reader.GetInt32(2);
            int sideA = reader.GetInt32(3);
            int size = reader.GetInt32(4);
            int prefabID = reader.GetInt32(5);

            if (sideA == 1)
            {
                GameObject obj = (GameObject)Instantiate(prefabs[prefabID], transform.position, transform.rotation);

                NormalAsset normalAsset = obj.GetComponent<NormalAsset>();
                normalAsset.posY = posY;
                normalAsset.posX = posX;
                normalAsset.posZ = 0;
                normalAsset.twoP = twoP;
                normalAsset.sideA = sideA;

                normalAsset.SetValues();
            }
            if (sideA == 0)
            {
                GameObject obj = (GameObject)Instantiate(prefabs[prefabID], transform.position, transform.rotation);

                NormalAsset normalAsset = obj.GetComponent<NormalAsset>();
                normalAsset.posY = posY;
                normalAsset.posX = 21 - posX;
                normalAsset.posZ = 0 + zOffset;
                normalAsset.twoP = twoP;
                normalAsset.sideA = sideA;

                normalAsset.SetValues();
            }

            
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    void ListExistingTables()
    {
        string conn = "URI=file:" + connectionString; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table';";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string table = reader.GetString(0);
            if (table != "sqlite_sequence")
            {
                existingTables.Add(table);
            }
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    bool CheckForDuplicate(string text)
    {
        foreach (string item in existingTables)
        {
            if (item == text)
            {
                checkForDuplicatesTXT.text = "Already exists";
                return true;
            }
        }
        checkForDuplicatesTXT.text = "OK";
        return false;
    }

    public void SetLevelName(string name)
    {
        if (!CheckForDuplicate(name))
        {
            levelName = name;
        }
        else
        {
            levelName = "";
        }
    }
}
