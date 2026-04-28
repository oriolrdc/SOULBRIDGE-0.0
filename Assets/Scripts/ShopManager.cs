using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ShopManager : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _SNCanva;
    [SerializeField] PlayerInput playerInput;
    float MejoraDeFuerza;
    float MejoraDeVelocidad;
    float MejoraDeDash;
    [SerializeField] Text _fuerzaTxt;
    [SerializeField] Text _velocidadTxt;
    [SerializeField] Text _dashTxt;
    [SerializeField] float reminingPoints;

    public void CheckRemainingPoints(string Type)
    {
        if(reminingPoints > 0)
        {
            SubirNivel(Type);
        }
    }

    void SubirNivel(string Type)
    {
        if(Type == "Fuerza")
        {
            if(MejoraDeFuerza < 5)
            {
                MejoraDeFuerza ++;
                _fuerzaTxt.text = MejoraDeFuerza.ToString() + "/5";
            }
        }
        if(Type == "Velocidad")
        {
            if(MejoraDeVelocidad < 5)
            {
                MejoraDeVelocidad ++;
                _velocidadTxt.text = MejoraDeVelocidad.ToString() + "/5";
            }
        }
        if(Type == "Dash")
        {
            if(MejoraDeDash < 5)
            {
                MejoraDeDash ++;
                _dashTxt.text = MejoraDeDash.ToString() + "/5";
            }
        }
    }

    public void ResetStats()
    {

    }

    public void Interacted()
    {
        _SNCanva.SetActive(true);
        playerInput.actions.Disable();
    }

    public void CloseSN()
    {
        _SNCanva.SetActive(false);
        playerInput.actions.Enable();
    }

}
