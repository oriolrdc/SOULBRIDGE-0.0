using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance { get; private set; }

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarseLaEscena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarseLaEscena;
    }

    private void AlCargarseLaEscena(Scene escena, LoadSceneMode modo)
    {
        if(escena.name != "MainMenu")
        {
            GameObject GUI = GameObject.Find("GUI");

            Debug.Log("Cargada la escena: " + escena.name);
            GameManager.Instance._coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
            UIManager.Instance._THBr = GUI.transform.Find("BaraVida/Personajes/THBr").gameObject;
            UIManager.Instance._CHBr = GUI.transform.Find("BaraVida/Personajes/CHBr").gameObject;
            UIManager.Instance._Rhb = GUI.transform.Find("BaraVida/Bars/RHB").GetComponent<Image>();
            UIManager.Instance._Lhb = GUI.transform.Find("BaraVida/Bars/LHB").GetComponent<Image>();
            UIManager.Instance._Ccb = GUI.transform.Find("BaraVida/Bars/CCB").GetComponent<Image>();
            UIManager.Instance._CHabilities = GUI.transform.Find("Habilities/CedricHabilities").gameObject;
            UIManager.Instance._THabilities = GUI.transform.Find("Habilities/ThalyaHabilities").gameObject;
            UIManager.Instance._CUlt = GUI.transform.Find("Habilities/CedricHabilities/CUlti").GetComponent<Image>();
            UIManager.Instance._CCPHab = GUI.transform.Find("Habilities/CedricHabilities/CPHab").GetComponent<Image>();
            UIManager.Instance._CBasic = GUI.transform.Find("Habilities/CedricHabilities/CBasic").GetComponent<Image>();
            UIManager.Instance._TUlt = GUI.transform.Find("Habilities/ThalyaHabilities/TUlti").GetComponent<Image>();
            UIManager.Instance._CTPHab = GUI.transform.Find("Habilities/ThalyaHabilities/TPHab").GetComponent<Image>();
            UIManager.Instance._TBasic = GUI.transform.Find("Habilities/ThalyaHabilities/TBasic").GetComponent<Image>();
        }
        
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
