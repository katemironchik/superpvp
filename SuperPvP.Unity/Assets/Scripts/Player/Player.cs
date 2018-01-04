using Assets.Scripts.Configs;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject targetDirection;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDirection != null)
        {
            var targetPossition = targetDirection.transform.position;
            targetPossition.y = gameObject.transform.position.y;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPossition, (float)0.1);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Tags.Drug)
        {
            var field = GameObject.Find(GameObjects.GameField).GetComponent<DrugGenerator>();
            field.DestroyDrug(collision.gameObject);
        }
    }

    public void MoveTo(GameObject targetDirection)
    {
        this.targetDirection = targetDirection;
    }

    public void Stop()
    {
        targetDirection = null;
    }
}