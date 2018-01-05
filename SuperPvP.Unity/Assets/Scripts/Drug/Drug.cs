using UnityEngine;

public class Drug : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        SetDrugRotation();
        SetDrugPosition();
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetDrugPosition()
    {
        var size = gameObject.GetComponent<Renderer>().bounds.size;
        transform.position = new Vector3(transform.position.x, size.y / 2, transform.position.z);
    }

    private void SetDrugRotation()
    {
        var vector = new Vector3(90, Random.Range(0, 360), 0);
        gameObject.transform.Rotate(vector);
    }
}