using Assets.Scripts.Configs;
using UnityEngine;

public class GameFieldInitializer : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {        
        var tilePrefab = Resources.Load<GameObject>(Prefabs.Tile);
        for (var i = 0; i < EnvironmentConfig.FieldSize; i++)
        {
            for (var j = 0; j < EnvironmentConfig.FieldSize; j++)
            {
                var tile = Instantiate(tilePrefab, gameObject.transform);
                tile.transform.position = new Vector3(i, 0, j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}