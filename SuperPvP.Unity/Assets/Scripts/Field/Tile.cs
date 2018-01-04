using Assets.Scripts.Configs;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool directionChosen;
    private GameObject player;

    public int i;
    public int j;
    
    void Update()
    {
    }

    void OnMouseDown()
    {
        HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Began);
    }

    void OnMouseUp()
    {
        HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Ended);
    }

    private void HandleTouch(int touchFingerId, Vector3 target, TouchPhase touchPhase)
    {
        switch (touchPhase)
        {
            case TouchPhase.Began:
                directionChosen = false;
                break;
            case TouchPhase.Ended:
                directionChosen = true;
                break;
        }

        if (directionChosen)
        {
            if (player == null)
            {
                player = GameObject.Find(GameObjects.Player);
            }
            if (player != null)
            {
                player.GetComponent<Player>().MoveTo(gameObject.transform.position);
            }
        }
    }
}
