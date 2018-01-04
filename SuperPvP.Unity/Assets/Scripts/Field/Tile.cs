using Assets.Scripts.Configs;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool directionChosen;

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
            var player = GameObject.Find(GameObjects.Player).GetComponent<Player>();
            player.MoveTo(gameObject);
        }
    }
}
