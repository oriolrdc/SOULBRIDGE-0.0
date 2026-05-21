using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Coins")]
    public int _coins;
    public Text _coinsText;
    public PlayerInput playerInputs;

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
        int cantidadDinero = Random.Range(10, 50);
        _coins += cantidadDinero;
        _coinsText.text = _coins.ToString();
    }

    public void EndLevel(string scene)
    {
        InputSystem.actions.FindActionMap("Player").Disable();
        //animation timeline + timer
        SceneManagerScript.Instance.LoadScene(scene);
    }

    public void ADInputs()
    {
        if (playerInputs.enabled) 
        {
            playerInputs.actions.Disable();
            playerInputs.enabled = false; // ¡Importante apagar el componente también!
        }
        // Si no (estaban desactivados), los activamos
        else 
        {
            playerInputs.actions.Enable();
            playerInputs.enabled = true; // ¡Importante encenderlo!
        }

    }
}
