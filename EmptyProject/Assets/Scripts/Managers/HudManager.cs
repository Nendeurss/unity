namespace STUDENT_NAME
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using SDD.Events;

	public class HudManager : Manager<HudManager>
	{

        //[Header("HudManager")]
        #region Labels & Values
        // TO DO
        [SerializeField] private Text m_TxtNLives;
        [SerializeField] private Text m_TxtScore;
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion

		#region Callbacks to GameManager events
		protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
		{
            //TO DO
            m_TxtScore.text = e.eScore.ToString();
            m_TxtNLives.text = e.eNLives.ToString();
        }
		#endregion

	}
}