using Assets.Scripts.Configs;
using UnityEngine;

public class DrugGenerator : MonoBehaviour
{
    private static GameObject drugPrefab;
    private int drugsAmount;

    // Use this for initialization
    void Start()
    {
        drugPrefab = Resources.Load<GameObject>(Prefabs.Drug);
    }

    // Update is called once per frame
    void Update()
    {
        if (drugsAmount < EnvironmentConfig.DrugsAmountLimit)
        {
            GenerateDrug();
        }
    }

    private void GenerateDrug()
    {
        var drug = Instantiate(drugPrefab);
        SetDrugRotation(drug);
        SetDrugPosition(drug);
        drug.GetComponent<MeshRenderer>().enabled = true;
        drugsAmount++;
    }

    private void SetDrugPosition(GameObject drug)
    {
        var size = drug.GetComponent<Renderer>().bounds.size;
        var vector = new Vector3(
            Random.Range(0, EnvironmentConfig.FieldSize - size.x),
            size.y / 2, 
            Random.Range(0, EnvironmentConfig.FieldSize - size.z));
        drug.transform.position += vector;
    }

    private void SetDrugRotation(GameObject drug)
    {
        var vector = new Vector3(90, Random.Range(0, 360), 0);
        drug.transform.Rotate(vector);
    }

    public void DestroyDrug(GameObject drug)
    {
        Destroy(drug);
        drugsAmount--;
    }
}