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
    string connectionString;

    public GameObject sideA;
    public GameObject sideB;

    public EditorAsset[] sideAObjects;
    public EditorAsset[] sideBObjects;

    public bool save;

    public GameObject[] prefabs;
    public Text levelNameText;

    EditorAsset editorasset;

    void Start()
    {
        connectionString = Application.dataPath + "/Database/FlipBoard.db";
        //CreateLevelTable();
        //AddToDataTable(1,2,3,4,5,6);
        sideAObjects = sideA.GetComponentsInChildren<EditorAsset>();
        //SpawnObjectsFromDatabase();
        save = false;
    }

    
    void Update()
    {
        if (save)
        {
            sideAObjects = sideA.GetComponentsInChildren<EditorAsset>();
            sideBObjects = sideA.GetComponentsInChildren<EditorAsset>();
            CreateLevelTable();
            for (int i = 0; i < sideAObjects.Length; i++)
            {
                AddToDataTable(sideAObjects[i].gridPosY, sideAObjects[i].gridPosX, Convert.ToInt32(sideAObjects[i].twoP), Convert.ToInt32(sideAObjects[i].sideA), (int)sideAObjects[i].size, (int)sideAObjects[i].asset);
            }

            save = false;
        }
    }

    public void Save()
    {
        levelName = levelNameText.text;
        save = true;
    }

    public void Load()
    {
        SpawnObjectsFromDatabase();
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

            GameObject obj = (GameObject)Instantiate(prefabs[0], transform.position, transform.rotation);

            NormalAsset normalAsset = obj.GetComponent<NormalAsset>();
            normalAsset.posY = posY;
            normalAsset.posX = posX;
            normalAsset.twoP = twoP;
            normalAsset.sideA = sideA;

            normalAsset.SetValues();

            //= reader.GetInt32(0);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
