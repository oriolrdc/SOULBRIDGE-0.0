using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndLevel(string scene)
    {
        InputSystem.actions.FindActionMap("Player").Disable();
        //animation timeline + timer
        SceneManagerScript.Instance.LoadScene(scene);
    }
}
