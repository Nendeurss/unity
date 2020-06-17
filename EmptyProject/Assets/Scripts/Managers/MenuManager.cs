
namespace LIM_TRAN_HOUACINE_NGUYEN
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using SDD.Events;

	public class MenuManager : Manager<MenuManager>
	{

		[Header("MenuManager")]

		#region Panels
		[Header("Panels")]
		[SerializeField] GameObject m_PanelMainMenu;
		[SerializeField] GameObject m_PanelInGameMenu;
        [SerializeField] GameObject m_PanelVictory;
		[SerializeField] GameObject m_PanelGameOver;
        [SerializeField] GameObject m_PanelTimer;

        List<GameObject> m_AllPanels;
        float timer;
		#endregion

		#region Events' subscription
		public override void SubscribeEvents()
		{
			base.SubscribeEvents();
            EventManager.Instance.AddListener<TimerBeforePlayEvent>(TimerBeforePlay);


        }

        public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();
            EventManager.Instance.RemoveListener<TimerBeforePlayEvent>(TimerBeforePlay);

        }
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion

		#region Monobehaviour lifecycle
		protected override void Awake()
		{
			base.Awake();
			RegisterPanels();
		}

		private void Update()
		{
			if (Input.GetButtonDown("Cancel") && GameManager.Instance.IsPlaying)
			{
				EscapeButtonHasBeenClicked();
			}
		}
		#endregion

		#region Panel Methods
		void RegisterPanels()
		{
			m_AllPanels = new List<GameObject>();
			m_AllPanels.Add(m_PanelMainMenu);
			m_AllPanels.Add(m_PanelInGameMenu);
			m_AllPanels.Add(m_PanelGameOver);
            m_AllPanels.Add(m_PanelVictory);
            m_AllPanels.Add(m_PanelTimer);

        }

		void OpenPanel(GameObject panel)
		{
			foreach (var item in m_AllPanels)
				if (item) item.SetActive(item == panel);
		}
		#endregion

		#region UI OnClick Events
		public void EscapeButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new EscapeButtonClickedEvent());
		}

		public void PlayButtonHasBeenClicked()
		{

            EventManager.Instance.Raise(new PlayButtonClickedEvent());
		}

		public void ResumeButtonHasBeenClicked()
		{
 
            EventManager.Instance.Raise(new ResumeButtonClickedEvent());
		}

		public void MainMenuButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
		}

		public void QuitButtonHasBeenClicked()
		{
            
			EventManager.Instance.Raise(new QuitButtonClickedEvent());
		}

        public void TimerBeforePlay()
        {
            EventManager.Instance.Raise(new TimerBeforePlayEvent());
        }

		#endregion

		#region Callbacks to GameManager events
		protected override void GameMenu(GameMenuEvent e)
		{
			OpenPanel(m_PanelMainMenu);
		}

		protected override void GamePlay(GamePlayEvent e)
		{
            //OpenPanel(null);
            EventManager.Instance.Raise(new TimerBeforePlayEvent());
        }

		protected override void GamePause(GamePauseEvent e)
		{
			OpenPanel(m_PanelInGameMenu);
		}

		protected override void GameResume(GameResumeEvent e)
		{
            //OpenPanel(null);
            EventManager.Instance.Raise(new TimerBeforePlayEvent());
        }

		protected override void GameOver(GameOverEvent e)
		{
			OpenPanel(m_PanelGameOver);
		}

        protected override void GameVictory(GameVictoryEvent e)
        {
            OpenPanel(m_PanelVictory);
        }


        protected void TimerBeforePlay(TimerBeforePlayEvent e)
        {
            OpenPanel(m_PanelTimer);
            StartCoroutine(StartCountdown());
        }

        IEnumerator StartCountdown()
        {
            Debug.Log("debut coroutine Menu manager");
            //m_GameState = GameState.gameTimer;
            timer = GameManager.Instance.timerStart;
            while (timer > 0)
            {
                Debug.Log(" MENU MANAGER TIMER " + timer);
                yield return new WaitForSecondsRealtime(1f); 
                timer--;
            }
            OpenPanel(null);
            Debug.Log("fin coroutine MENU manager");

        }
        #endregion
    }

}
