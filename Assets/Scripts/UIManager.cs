using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] Image _Rhb;
    [SerializeField] Image _Lhb;
    [SerializeField] Image _Ccb;

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

    public void UpdateHB(float damage)
    {
        _Rhb.fillAmount = damage;
        _Lhb.fillAmount = damage;
    }

    public void UpdateChangeBar()
    {
        float tiempo = 1;
        float tiempoTranscurrido = 0;
        _Ccb.fillAmount = 0;

        while (tiempoTranscurrido < tiempo)
        {
            tiempoTranscurrido += Time.deltaTime;
            _Ccb.fillAmount = tiempoTranscurrido / tiempo;
        }

        _Ccb.fillAmount = 1f;
    }
}
