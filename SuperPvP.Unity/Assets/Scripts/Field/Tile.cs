using Assets.Scripts.Configs;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool directionChosen;
    private GameObject player;
    private ServerCommandSender commandSender;

    public int i;
    public int j;

    void Start()
    {
        commandSender = GameObject.Find(GameObjects.ServerTransport).GetComponent<ServerCommandSender>();
    }

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
            commandSender.SendPlayerMoveData(i, j);
            /*if (player == null)
            {
                player = GameObject.Find(GameObjects.Player);
            }
            if (player != null)
            {
                player.GetComponent<Player>().MoveTo(gameObject.transform.position);
            }*/
        }
    }
}
