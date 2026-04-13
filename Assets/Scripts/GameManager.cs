using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Coins")]
    [SerializeField] int _coins;

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

    public void AddCoins()
    {
        _coins++;
    }

    public void EndLevel(string scene)
    {
        InputSystem.actions.FindActionMap("Player").Disable();
        //animation timeline + timer
        SceneManagerScript.Instance.LoadScene(scene);
    }
}
