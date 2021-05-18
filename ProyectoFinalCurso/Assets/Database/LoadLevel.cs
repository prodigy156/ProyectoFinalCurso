﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System;

public class LoadLevel : MonoBehaviour
{

    public Dropdown dropdown;

    public string levelName;
    string connectionString;

    public float zOffset;

    public List<string> existingTables;

    public GameObject[] objectsToHide;

    public GameObject[] prefabs;

    void Start()
    {
        connectionString = Application.dataPath + "/Database/FlipBoard.db";
        ListExistingTables();
    }

    public void SpawnObjectsFromDatabase()
    {
        for (int i = 0; i < objectsToHide.Length; i++)
        {
            objectsToHide[i].SetActive(false);
        }

        string conn = "URI=file:" + connectionString; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT PosY, PosX, TwoP, SideA, Size, PrefabID " + "FROM " + levelName + " WHERE 1";
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
                if (twoP != 1)
                {
                    GameObject obj = (GameObject)Instantiate(prefabs[prefabID], transform.position, transform.rotation);

                    NormalAsset normalAsset = obj.GetComponent<NormalAsset>();
                    normalAsset.posY = posY;
                    normalAsset.posX = posX;
                    normalAsset.posZ = 0 + (zOffset/2);
                    normalAsset.twoP = twoP;
                    normalAsset.sideA = sideA;

                    normalAsset.SetValues();
                }
                else
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
                
            }
            if (sideA == 0)
            {
                if (twoP != 1)
                {
                    GameObject obj = (GameObject)Instantiate(prefabs[prefabID], transform.position, transform.rotation);

                    NormalAsset normalAsset = obj.GetComponent<NormalAsset>();
                    normalAsset.posY = posY;
                    normalAsset.posX = 21 - posX;
                    normalAsset.posZ = 0 - (zOffset / 2);
                    normalAsset.twoP = twoP;
                    normalAsset.sideA = sideA;

                    normalAsset.SetValues();
                }
                else
                {
                    GameObject obj = (GameObject)Instantiate(prefabs[prefabID], transform.position, transform.rotation);

                    NormalAsset normalAsset = obj.GetComponent<NormalAsset>();
                    normalAsset.posY = posY;
                    normalAsset.posX = 21 - posX;
                    normalAsset.posZ = 0;
                    normalAsset.twoP = twoP;
                    normalAsset.sideA = sideA;

                    normalAsset.SetValues();
                }
                
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

        dropdown.ClearOptions();
        dropdown.AddOptions(existingTables);
        levelName = existingTables[0];

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void SetLevelName(int i)
    {
        levelName = existingTables[i];
    }
}
