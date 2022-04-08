using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}

[System.Serializable]
public class SavedData
{
    public int[] inv;
    public int[] upgrades;
    public int money;
    public SerializableVector3 universePoint;
    public SerializableVector3 playerPoint;
    public int currentArtifact;
    public float fuel;
    public float integrity;
    public float voidShield;
    public int questProgress;
    public double time;

    public SavedData(int[] _inv, int[] _u, int _mon, Vector3 _uP, Vector3 _pP, int _cA, float _f, float _i, float _vS, int _qP, double _time)
    {
        inv = _inv;
        upgrades = _u;
        money = _mon;
        universePoint = _uP;
        playerPoint = _pP;
        currentArtifact = _cA;
        fuel = _f;
        integrity = _i;
        voidShield = _vS;
        questProgress = _qP;
        time = _time;
    }
}


public static class SaveManager : object
{
    public static void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        SavedData save = new SavedData(Inventory.instance.GetIDArray(), UpgradeManager.instance.upgrades, 
            GameManager.instance.money, 
            GameManager.instance.universe.transform.position, 
            GameManager.instance.player.transform.position, 
            GameManager.instance.current_verse, 
            GameManager.instance.player.GetComponent<spaceship>().fuel, 
            GameManager.instance.player.GetComponent<spaceship>().integrity, 
            GameManager.instance.player.GetComponent<spaceship>().voidShield, 
            GameManager.instance.questProgress,
            GameManager.instance.time);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, save);
        file.Close();
    }

    public static void ResetFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";

        if (File.Exists(destination)) File.Delete(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }
    }

    public static bool LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file = null;

        if (!File.Exists(destination) || new FileInfo(destination).Length == 0)
        {
            Debug.LogError("File not found");
            return false;
        }
        else
        {
            file = File.OpenRead(destination);
        }

        BinaryFormatter bf = new BinaryFormatter();
        SavedData data= null;
        try
        {
            data = (SavedData)bf.Deserialize(file);
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
            return false;
        }
        file.Close();

        for (int i = 0; i < data.inv.Length; i++)
        {
            Inventory.instance.AddItem(data.inv[i]);
        }
        UpgradeManager.instance.upgrades = data.upgrades;
        UpgradeManager.instance.UpdateStats();
        GameManager.instance.money = data.money;
        GameManager.instance.universe.transform.position = data.universePoint;
        GameManager.instance.player.transform.position = data.playerPoint;
        GameManager.instance.current_verse = data.currentArtifact;
        for (int i = 0; i < data.currentArtifact; i++)
        {
            GameManager.instance.artifacts_found[i] = true;
        }

        GameManager.instance.player.GetComponent<spaceship>().fuel = data.fuel;
        GameManager.instance.player.GetComponent<spaceship>().integrity = data.integrity;
        GameManager.instance.player.GetComponent<spaceship>().voidShield = data.voidShield;
        GameManager.instance.questProgress = data.questProgress;
        GameManager.instance.time = data.time;

        return true;
    }
}
