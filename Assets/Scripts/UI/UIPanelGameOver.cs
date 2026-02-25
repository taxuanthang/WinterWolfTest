using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelGameOver : MonoBehaviour, IMenu
{
    [SerializeField] private Button btnRestart;
    [SerializeField] private Button btnReturnHome;

    private UIMainManager m_mngr;

    private void Awake()
    {
        btnRestart.onClick.AddListener(OnClickClose);
        btnReturnHome.onClick.AddListener(OnClickRestart);
    }

    private void OnDestroy()
    {
        if (btnRestart) btnRestart.onClick.RemoveAllListeners();
        if (btnReturnHome) btnRestart.onClick.RemoveAllListeners();
    }

    private void OnClickClose()
    {
        m_mngr.ShowMainMenu();
    }

    private void OnClickRestart()
    {
        m_mngr.ShowMainMenu();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

}
