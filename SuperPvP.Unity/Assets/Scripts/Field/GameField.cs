using Assets.Scripts.Configs;
using SuperPvP.Core.Server.Models;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    private Dictionary<GameObjectType, GameObject> prefabs;
    public int PlayerId;
    private GameObject[,] tiles = new GameObject[EnvironmentConfig.FieldSize, EnvironmentConfig.FieldSize];
    private Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();

    // Use this for initialization
    void Start()
    {
        FillPrefabs();
        var tilePrefab = Resources.Load<GameObject>(Prefabs.Tile);
        for (var i = 0; i < EnvironmentConfig.FieldSize; i++)
        {
            for (var j = 0; j < EnvironmentConfig.FieldSize; j++)
            {
                var tile = Instantiate(tilePrefab, gameObject.transform);
                var tileScript = tile.GetComponent<Tile>();
                tileScript.i = i;
                tileScript.j = j;
                tile.transform.position = new Vector3(i, 0, j);
                tiles[i, j] = tile;
            }
        }
    }

    private void FillPrefabs()
    {
        prefabs = new Dictionary<GameObjectType, GameObject>();
        prefabs.Add(GameObjectType.Player, Resources.Load<GameObject>(Prefabs.Player));
        prefabs.Add(GameObjectType.Drug, Resources.Load<GameObject>(Prefabs.Drug));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshFieldFromServer(List<ServerGameObject> serverData)
    {
        foreach (var go in serverData)
        {
            GenerateGameObject(go);
        }
    }

    public void SetPlayerIdentifier(int serverData)
    {
        PlayerId = serverData;
    }

    private void GenerateGameObject(ServerGameObject serverGameObject)
    {
        var serverToGamePosition = tiles[serverGameObject.Position.I, serverGameObject.Position.J].transform.position;
        GameObject go;
        if (objects.ContainsKey(serverGameObject.Id))
        {
            go = objects[serverGameObject.Id];
            if (serverGameObject.Type == GameObjectType.Player || serverGameObject.Type == GameObjectType.Enemy)
            {
                go.GetComponent<Player>().MoveTo(serverToGamePosition);
            }
        }
        else
        {
            go = Instantiate(prefabs[serverGameObject.Type]);
            objects.Add(serverGameObject.Id, go);
            go.transform.position = serverToGamePosition;
            go.GetComponent<ServerIdKeeper>().ServerId = serverGameObject.Id;
        }
    }
}