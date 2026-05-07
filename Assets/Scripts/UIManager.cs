using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject _THBr;
    public GameObject _CHBr;
    public Image _Rhb;
    public Image _Lhb;
    public Image _Ccb;
    public GameObject _CHabilities;
    public GameObject _THabilities;

    public Image _CUlt;
    public Image _CCPHab;
    public Image _CBasic;
    public Image _TUlt;
    public Image _CTPHab;
    public Image _TBasic;

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
        _Ccb.fillAmount = 0;
        _Ccb.DOFillAmount(1f, 1f);
    }

    public void ChangeHBandHab()
    {
        if(_THBr.activeInHierarchy)
        {
            _CHBr.SetActive(true);
            _THBr.SetActive(false);
            _CHabilities.SetActive(true);
            _THabilities.SetActive(false);
            DOTween.Restart("CBarra");
        }
        else if(_CHBr.activeInHierarchy)
        {
            _THBr.SetActive(true);
            _CHBr.SetActive(false);
            _THabilities.SetActive(true);
            _CHabilities.SetActive(false);
            DOTween.Restart("TBarra");
        }
    }

    public void CCUlt()
    {
        _CUlt.fillAmount = 0;
        _CUlt.DOFillAmount(1f, 30f);
    }

    public void CTUlt()
    {
        _TUlt.fillAmount = 0;
        _TUlt.DOFillAmount(1f, 25f);
    }

    public void CCPHab()
    {
        _CCPHab.fillAmount = 0;
        _CCPHab.DOFillAmount(1f, 10f);
    }

    public void CTPHab()
    {
        _CTPHab.fillAmount = 0;
        _CTPHab.DOFillAmount(1f, 5f);
    }

    public void CCBasic(float time)
    {
        _CBasic.fillAmount = 0;
        _CBasic.DOFillAmount(1f, time);
    }

    public void CTBasic()
    {
        _TBasic.fillAmount = 0;
        _TBasic.DOFillAmount(1f, 0.2f);
    }


}
