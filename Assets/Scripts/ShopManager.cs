using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour, IInteractable
{
    public ScriptableItems[] mejoras;
    public Image[] iconos;
    public Text[] price;
    public Image inspectImagen;
    public Text itemName;
    public Text itemDescription;

    public GameObject shopCanvas;
    public ScriptableItems itemSeleccionado;

    void Start()
    {
        foreach(ScriptableItems item in mejoras)
        {
            item.comprada = false;
        }
    }

    void OnEnable()
    {
        RellenarInfo();
    }

    void RellenarInfo()
    {
        for (int i = 0; i < mejoras.Length; i++)
        {
            iconos[i].sprite = mejoras[i].icono;
            price[i].text = mejoras[i].price.ToString();
        }
    }

    public void RellenarInspeccion(ScriptableItems item)
    {
        inspectImagen.sprite = item.icono;
        itemName.text = item.nombre;
        itemDescription.text = item.descripcion;
        itemSeleccionado = item;
    }

    public void CloseShop()
    {
        shopCanvas.SetActive(false);
        GameManager.Instance.ADInputs();
    }

    public void Interacted()
    {
        shopCanvas.SetActive(true);
        GameManager.Instance.ADInputs();
    }

    public void TryToBuy()
    {
        if(itemSeleccionado.nombre == "Reflejos de Mercurio" && !itemSeleccionado.comprada && GameManager.Instance._coins >= itemSeleccionado.price)
        {
            GameManager.Instance._coins -= itemSeleccionado.price;
            PlayerData.Instance.dashCooldown = 0.2f;
            itemSeleccionado.comprada = true;
            Debug.Log("has comprado" + itemSeleccionado.nombre);
        }
        if(itemSeleccionado.nombre == "Calibre de Impacto" && !itemSeleccionado.comprada && GameManager.Instance._coins >= itemSeleccionado.price)
        {
            GameManager.Instance._coins -= itemSeleccionado.price;
            PlayerData.Instance.BulletDamage = 20;
            itemSeleccionado.comprada = true;
            Debug.Log("has comprado" + itemSeleccionado.nombre);
        }
        if(itemSeleccionado.nombre == "Filo de Espinas" && !itemSeleccionado.comprada && GameManager.Instance._coins >= itemSeleccionado.price)
        {
            GameManager.Instance._coins -= itemSeleccionado.price;
            PlayerData.Instance.AttackDamage = 20;
            itemSeleccionado.comprada = true;
            Debug.Log("has comprado" + itemSeleccionado.nombre);
        }
        if(itemSeleccionado.nombre == "Zancada del Viento" && !itemSeleccionado.comprada && GameManager.Instance._coins >= itemSeleccionado.price)
        {
            GameManager.Instance._coins -= itemSeleccionado.price;
            PlayerData.Instance.moveSpeed = 10;
            itemSeleccionado.comprada = true;
            Debug.Log("has comprado" + itemSeleccionado.nombre);
        }
        if(itemSeleccionado.nombre == "Coraza Espiritual" && !itemSeleccionado.comprada && GameManager.Instance._coins >= itemSeleccionado.price)
        {
            GameManager.Instance._coins -= itemSeleccionado.price;
            PlayerData.Instance._maxHealth = 120;
            PlayerData.Instance._Health = 120;
            itemSeleccionado.comprada = true;
            Debug.Log("has comprado" + itemSeleccionado.nombre);
        }
    }


}
