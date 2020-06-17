namespace LIM_TRAN_HOUACINE_NGUYEN
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
        [SerializeField] public GameObject firstSpawnPos;
        [SerializeField] public Transform actualChunkPos;
        [SerializeField] private Transform nextChunkPos;
        [SerializeField] private GameObject NextChunkTriggerPrefab;

        [Header("Player")]
        [SerializeField] public GameObject PlayerPrefab;
        //[SerializeField] private Transform PlayerSpawnPoint;

        private GameObject newPlayer;
        private List<GameObject> chunkList = new List<GameObject>();
        private Transform PlayerPos;
        private GameObject chunkTrigger;

        private void Reset()
        {
            Destroy(newPlayer);
            while (chunkList.Count > 0)
            {
                Destroy(chunkList[chunkList.Count - 1]);
                chunkList.RemoveAt(chunkList.Count - 1);
            }

            resetChunkPosition();

            //Debug.Log(chunkPrefab.GetComponent<Renderer>().bounds.size);
            //Debug.Log(chunkPrefab.GetComponent<Collider>().bounds.size);
            newPlayer = Instantiate(PlayerPrefab, nextChunkPos.position, Quaternion.identity);
            PlayerPos = newPlayer.transform;
            for (int i = 0; i < 10; i++)
            {
                GameObject newChunk = Instantiate(chunkPrefab, nextChunkPos.position, Quaternion.identity);
                chunkList.Add(newChunk);
                UpdateActualChunkPos(nextChunkPos);
                UpdateNextChunkPos();
            }
            if(chunkTrigger!=null) Destroy(chunkTrigger);
            chunkTrigger = createChunkTrigger();

            PlayerInChunk = 2;


        }

        #region Manager implementation
        protected override IEnumerator InitCoroutine()
		{
            
            yield break;
        }
        #endregion

        #region Chunk Managment
        private GameObject createChunkTrigger()
        {
            GameObject chunk = Instantiate(NextChunkTriggerPrefab, chunkList[5].transform.position, Quaternion.identity);
            
            Vector3 chunkCollider = chunk.GetComponent<BoxCollider>().size;
            chunkCollider.x = chunkPrefab.GetComponent<Renderer>().bounds.size.x*10;
            chunkCollider.y = 500;
            chunkCollider.z = chunkPrefab.GetComponent<Renderer>().bounds.size.z;
            chunk.transform.localScale = chunkCollider;
            return chunk;
        }

        private void resetChunkPosition()
        {
            this.actualChunkPos.position= firstSpawnPos.transform.position;
            this.nextChunkPos.position = firstSpawnPos.transform.position;
        }
        private void UpdateNextChunkPos()
        {
            this.nextChunkPos.position = new Vector3(0, 0, nextChunkPos.position.z + chunkPrefab.GetComponent<Renderer>().bounds.size.z);
            //return nextChunkPos;
        }

        private void UpdateActualChunkPos(Transform nextChunkPos)
        {
            this.actualChunkPos.position = new Vector3(nextChunkPos.position.x, nextChunkPos.position.y, nextChunkPos.position.z);
            //return actualChunkPos;
        }

        private void addNewChunkIn()
        {
            GameObject newChunk = Instantiate(chunkPrefab, nextChunkPos.position, Quaternion.identity);
            chunkList.Add(newChunk);
            UpdateActualChunkPos(nextChunkPos);
            UpdateNextChunkPos();
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
            }
        }

        private void ChunkTriggerDestroyed(ChunkTriggerDestroyed e)
        {
            chunkTrigger = createChunkTrigger();
        }
        #endregion

        protected override void GameMenu(GameMenuEvent e)
		{
            Reset();
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


    }


}