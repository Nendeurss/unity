namespace STUDENT_NAME
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.Linq;
	using SDD.Events;

	public class LevelManager : Manager<LevelManager>
	{

        private bool PlayerInNewChunk = true;
        //Compteur de chunk traversé par le player
        private int PlayerInChunk = 0;

        [Header("Management des chunks")]
        [Tooltip("Vitesse de création d'un nouveau chunk au bout de la map")]
        [SerializeField] float m_chunkSpawnSpeed;
        [SerializeField] public GameObject chunkPrefab;
        [SerializeField] public Transform actualChunkPos;
        [SerializeField] public GameObject PlayerPrefab;
        [SerializeField] private Transform nextChunkPos;
        [SerializeField] private Transform PlayerSpawnPoint;
        [SerializeField] private GameObject NextChunkTriggerPrefab;

        private List<GameObject> chunkList = new List<GameObject>();
        private Transform PlayerPos;


        #region Manager implementation
        protected override IEnumerator InitCoroutine()
		{
            //Debug.Log(chunkPrefab.GetComponent<Renderer>().bounds.size);
            //Debug.Log(chunkPrefab.GetComponent<Collider>().bounds.size);
            for (int i = 0; i < 10; i++)
            {
                GameObject newChunk = Instantiate(chunkPrefab, nextChunkPos.position, Quaternion.identity);
                chunkList.Add(newChunk);
                actualChunkPos = UpdateActualChunkPos(actualChunkPos, nextChunkPos);
                nextChunkPos = UpdateNextChunkPos(nextChunkPos);
            }
            GameObject newChunkTrigger = createChunkTrigger();
            
            //Debug.Log("Actual Chunk Pos : " + actualChunkPos.position);
            //Debug.Log("Next chunk pos : " + nextChunkPos.position);
            PlayerInChunk = 2;

            GameObject newPlayer = Instantiate(PlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
            PlayerPos = newPlayer.transform;
           // Debug.Log(newPlayer.transform.position);
            //Debug.Log(chunkList[4].transform.position.z);

            yield break;
		}
        #endregion

        private GameObject createChunkTrigger()
        {
            GameObject chunk = Instantiate(NextChunkTriggerPrefab, chunkList[5].transform.position, Quaternion.identity);
            
            Vector3 chunkCollider = chunk.GetComponent<BoxCollider>().size;
            chunkCollider.x = chunkPrefab.GetComponent<Renderer>().bounds.size.x;
            chunkCollider.y = 200;
            chunkCollider.z = chunkPrefab.GetComponent<Renderer>().bounds.size.z;
            chunk.transform.localScale = chunkCollider;
            return chunk;
        }

        private Transform UpdateNextChunkPos(Transform nextChunkPos)
        {
            nextChunkPos.position = new Vector3(0, 0, nextChunkPos.position.z + chunkPrefab.GetComponent<Renderer>().bounds.size.z);
            return nextChunkPos;
        }

        private Transform UpdateActualChunkPos(Transform actualChunkPos, Transform nextChunkPos)
        {
            actualChunkPos.position = new Vector3(nextChunkPos.position.x, nextChunkPos.position.y, nextChunkPos.position.z);
            return actualChunkPos;
        }

        private void addNewChunkIn()
        {
            GameObject newChunk = Instantiate(chunkPrefab, nextChunkPos.position, Quaternion.identity);
            chunkList.Add(newChunk);
            actualChunkPos = UpdateActualChunkPos(actualChunkPos, nextChunkPos);
            nextChunkPos = UpdateNextChunkPos(nextChunkPos);
        }

        private void DeleteLastChunk()
        {
            Destroy(chunkList[0]);
            chunkList.RemoveAt(0);
        }

        private void PlayerHasReachedEndChunk(PlayerHasReachedEndChunk e)
        {
            if (chunkList.Count >= 10)
            {
                
                    Debug.Log("Here");
                    addNewChunkIn();
                    DeleteLastChunk();
                    Debug.Log(PlayerInNewChunk);
                
                //if ((int)PlayerPos.position.z / (int)chunkPrefab.GetComponent<Renderer>().bounds.size.z > PlayerInChunk && PlayerInNewChunk == false)
                //{

                //    PlayerInNewChunk = true;
                //    PlayerInChunk += 1;
                //    Debug.Log("Chunk nb : " + PlayerInChunk);
                //}
            }
        }

        private void ChunkTriggerDestroyed(ChunkTriggerDestroyed e)
        {
            GameObject newChunkTrigger = createChunkTrigger();
        }

        public override void SubscribeEvents()
		{
			base.SubscribeEvents();
            EventManager.Instance.AddListener<PlayerHasReachedEndChunk>(PlayerHasReachedEndChunk);
            EventManager.Instance.AddListener<ChunkTriggerDestroyed>(ChunkTriggerDestroyed);
        }

		public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();
            EventManager.Instance.RemoveListener<PlayerHasReachedEndChunk>(PlayerHasReachedEndChunk);
            EventManager.Instance.RemoveListener<ChunkTriggerDestroyed>(ChunkTriggerDestroyed);
        }

		protected override void GamePlay(GamePlayEvent e)
		{

		}

		protected override void GameMenu(GameMenuEvent e)
		{

		}

	}
}