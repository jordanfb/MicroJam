using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class World : MonoBehaviour {

    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tile[] tiles; // this stores what tiles go where I guess?

    private TileData[,] tileArray;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadWorld(string filename)
    {
        if (File.Exists(Application.persistentDataPath + filename))
        {
            // then load it
            string jsonString = File.ReadAllText(Application.persistentDataPath + filename);
            // do something with the json file:
            SaveGameData gameData = JsonUtility.FromJson<SaveGameData>(jsonString);
            // then apply that savegamedata to the world
        } else
        {
            // error
        }
    }

    void SaveWorld(string filename, SaveGameData gameData)
    {
        string dataAsJson = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + filename, dataAsJson);
    }
}


public class TileData
{
    // this stores tile data and shit like that?
    public float zRotation = 0;
    public int tileType = 0; // whatever I guess?
}


[System.Serializable]
public class SaveGameData
{
    public int worldWidth = 0;
    public int worldHeight = 0;
    public int[] tiles;
    public float[] tileRotations;
    // then store other things as well...
}