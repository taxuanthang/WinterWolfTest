using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelPause : MonoBehaviour, IMenu
{
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnReturnHome;

    private UIMainManager m_mngr;

    private void Awake()
    {
        btnClose.onClick.AddListener(OnClickClose);
        btnReturnHome.onClick.AddListener(OnClickReturnHome);
    }

    private void OnDestroy()
    {
        if (btnClose) btnClose.onClick.RemoveAllListeners();
        if (btnReturnHome) btnReturnHome.onClick.RemoveAllListeners();
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
    }

    private void OnClickClose()
    {
        m_mngr.ShowGameMenu();
    }
    private void OnClickReturnHome()
    {
        m_mngr.ShowMainMenu();
    }


    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
