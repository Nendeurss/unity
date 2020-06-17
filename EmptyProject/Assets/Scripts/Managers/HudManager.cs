namespace LIM_TRAN_HOUACINE_NGUYEN
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using SDD.Events;

    public class HudManager : Manager<HudManager>
    {

        [Header("HudManager")]
        #region Labels & Values
        // TO DO
        [SerializeField] private Text m_TxtNLives;
        [SerializeField] private Text m_TxtScore;
        [SerializeField] private Text m_TxtBestScore;
        [SerializeField] private Text m_TxtTimer;
        private float timer;
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine()
        {
            yield break;
        }
        #endregion

        #region Events subscription
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

        #region Callbacks to GameManager events
        protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
        {
            //TO DO
            m_TxtScore.text = e.eScore.ToString();
            m_TxtNLives.text = e.eNLives.ToString();
            m_TxtBestScore.text = e.eBestScore.ToString();
        }

        protected void TimerBeforePlay(TimerBeforePlayEvent e)
        {
            StartCoroutine(StartCountdown());
        }

        IEnumerator StartCountdown()
        {
            Debug.Log("debut coroutine HUD manager");
            //m_GameState = GameState.gameTimer;
            timer = GameManager.Instance.timerStart;
            while (timer > 0)
            {
                Debug.Log(" HUD MANAGER TIMER " + timer);
                m_TxtTimer.text = timer.ToString();
                yield return new WaitForSecondsRealtime(1f);
                timer--;
            }
            Debug.Log("fin coroutine HUD manager");

        }

        #endregion

    }    
}