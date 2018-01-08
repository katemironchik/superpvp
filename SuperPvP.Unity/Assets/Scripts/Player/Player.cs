using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3? targetDirection;

    public bool IsEnemy;

    // Use this for initialization
    void Start()
    {
        var size = gameObject.GetComponent<Renderer>().bounds.size;
        transform.position = new Vector3(transform.position.x, size.y / 2, transform.position.z);
        if (IsEnemy)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDirection.HasValue)
        {
            var targetPossition = targetDirection.GetValueOrDefault();
            targetPossition.y = gameObject.transform.position.y;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPossition, (float)0.1);
        }
    }

    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Tags.Drug)
        {
            var field = GameObject.Find(GameObjects.GameField).GetComponent<DrugGenerator>();
            field.DestroyDrug(collision.gameObject);
        }
    }*/

    public void MoveTo(Vector3 targetDirection)
    {
        this.targetDirection = targetDirection;
    }

    public void Stop()
    {
        targetDirection = null;
    }
}