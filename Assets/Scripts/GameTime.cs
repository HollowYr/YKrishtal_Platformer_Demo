using UnityEngine;

public class GameTime : MonoBehaviour
{
    public static GameTime Instance { get; private set; }
    public bool isPaused { get; set; }
    public float deltaTime { get { return isPaused ? 0 : Time.deltaTime; } }
    public float fixedDeltaTime { get { return isPaused ? 0 : Time.fixedDeltaTime; } }

    private void Start()
    {
        Instance = this;
        isPaused = false;
    }
}
