using DG.Tweening;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<eStateGame> StateChangedAction = delegate { };

    public enum eLevelMode
    {
        TIMER,
        MOVES
    }

    public enum eStateGame
    {
        SETUP,
        MAIN_MENU,
        GAME_STARTED,
        PAUSE,
        GAME_OVER,
    }

    private eStateGame m_state;
    public eStateGame State
    {
        get { return m_state; }
        private set
        {
            m_state = value;

            StateChangedAction(m_state);
        }
    }

    [SerializeField] private GameSettings m_gameSettings;

    [Header("Manager")]

    [SerializeField] private ItemSkinDatabase m_itemSkinDatabase;

    [SerializeField] private PrefabDatabase m_prefabDatabase;

    [SerializeField] private SoundManager m_soundManager;

    private BoardController m_boardController;

    [SerializeField] private UIMainManager m_uiMenu;

    private LevelCondition m_levelCondition;

    [Header("Commons")]
    [SerializeField] private ItemPool m_itemPool;


    [Header("Level stats")]
    eLevelMode m_currentLevelMode;

    public eLevelMode CurrentLevelMode
    {
        get { return m_currentLevelMode; }
        set { m_currentLevelMode = value; }
    }




    private void Awake()
    {
        State = eStateGame.SETUP;
        if (m_gameSettings == null)
        {
            m_gameSettings = Resources.Load<GameSettings>(Constants.GAME_SETTINGS_PATH);
        }

        if (m_uiMenu == null)
        {
            m_uiMenu = FindObjectOfType<UIMainManager>();
        }
        m_uiMenu.Setup(this);
    }

    void Start()
    {
        State = eStateGame.MAIN_MENU;
        m_soundManager.PlayBackgroundSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_boardController != null) m_boardController.Tick();
    }


    internal void SetState(eStateGame state)
    {
        State = state;

        if(State == eStateGame.PAUSE)
        {
            DOTween.PauseAll();
        }
        else
        {
            DOTween.PlayAll();
        }
    }

    public void LoadLevel(eLevelMode mode)
    {
        m_boardController = new GameObject("BoardController").AddComponent<BoardController>();
        m_boardController.StartGame(this, m_gameSettings,m_itemSkinDatabase,m_prefabDatabase,m_itemPool);

        if (mode == eLevelMode.MOVES)
        {
            m_levelCondition = this.gameObject.AddComponent<LevelMoves>();
            m_levelCondition.Setup(m_gameSettings.LevelMoves, m_uiMenu.GetLevelConditionView(), m_boardController);
        }
        else if (mode == eLevelMode.TIMER)
        {
            m_levelCondition = this.gameObject.AddComponent<LevelTime>();
            m_levelCondition.Setup(m_gameSettings.LevelMoves, m_uiMenu.GetLevelConditionView(), this);
        }

        CurrentLevelMode = mode;
        m_levelCondition.ConditionCompleteEvent += GameOver;

        State = eStateGame.GAME_STARTED;
    }

    public void GameOver()
    {
        StartCoroutine(WaitBoardController());
    }

    internal void ClearLevel()
    {
        if (m_boardController)
        {
            m_boardController.Clear();
            Destroy(m_boardController.gameObject);
            m_boardController = null;
        }
    }

    private IEnumerator WaitBoardController()
    {
        while (m_boardController.IsBusy)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        State = eStateGame.GAME_OVER;

        if (m_levelCondition != null)
        {
            m_levelCondition.ConditionCompleteEvent -= GameOver;

            Destroy(m_levelCondition);
            m_levelCondition = null;
        }

        SoundManager.instance.PlayWinSound();
    }
}
