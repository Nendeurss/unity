using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STUDENT_NAME;
using SDD.Events;

public class PlayerController : SimpleGameStateObserver, IScore
{
    Rigidbody m_Rigidbody;

    [Header("Deplacement")]
    [Tooltip("Vitesse en m.s-1")]
    [SerializeField] float m_ForwardSpeed;
    public float Speed { get { return m_ForwardSpeed; } }

    [SerializeField] float m_TranslationSpeed;
    [SerializeField] float m_Acceleration;
    [SerializeField] float m_TimeBetweenAcceleration;
    float timeBeforeAcceleration;


    [Header("Tire")]
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField] private float m_ShootPeriod;
    private float m_NextShootTime;
    [SerializeField] private Transform m_BulletSpawnPoint;
    [SerializeField] private float m_LifeDuration;
    [SerializeField] private float m_BulletSpeed;
    bool canJump;

    [Header("Score")]
    [SerializeField] private int m_Score;
    public int Score { get { return m_Score; } }

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody>();
        timeBeforeAcceleration = m_TimeBetweenAcceleration;
        canJump = true;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;
        if (m_Rigidbody.transform.position.y < 0)
        {
            EventManager.Instance.Raise(new GameOverEvent());
        }
        Debug.Log("Is playing : " + GameManager.Instance.IsPlaying);
        timeBeforeAcceleration -= Time.deltaTime;
        
        if (timeBeforeAcceleration < 0)
        {

            m_ForwardSpeed += m_Acceleration;
            timeBeforeAcceleration = m_TimeBetweenAcceleration;
        }
        if (Input.GetButton("Fire1") && m_NextShootTime < Time.time)
        {
            ShootBullet();
            m_NextShootTime = Time.time + m_ShootPeriod;
        }
        EventManager.Instance.Raise(new ScoreItemEvent() { eScore = this as IScore });
    }

    IEnumerator JumpCoroutine()
    {
        m_Rigidbody.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        canJump = false;
        while(transform.position.y!=1)yield return new WaitForSeconds(0.2f);
        canJump = true;
    }

    private void FixedUpdate()
    {

        // comportement dynamique cinétique (non-kinematic)
        // Time.fixedDeltaTime
        if (!GameManager.Instance.IsPlaying) return;
   

        //float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        Vector3 horizontalVect = transform.right * m_TranslationSpeed * hInput* Time.fixedDeltaTime;
        Vector3 forwardVect = transform.forward * m_ForwardSpeed * Time.fixedDeltaTime;
        m_Rigidbody.MovePosition(transform.position + forwardVect + horizontalVect);

        if (canJump && Input.GetKey("space"))
        {
            Debug.Log("JUMP");
            StartCoroutine(JumpCoroutine());
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(name + " Collision with " + collision.gameObject.name);
            EventManager.Instance.Raise(new PlayerHasBeenHitEvent() { ePlayerController = this });
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("EndChunk"))
        {
            EventManager.Instance.Raise(new PlayerHasReachedEndChunk() {});
            Destroy(collider.gameObject);
            EventManager.Instance.Raise(new ChunkTriggerDestroyed() { });
        }
    }

    void ShootBullet()
    {
        GameObject bulletGO = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
        bulletGO.GetComponent<Rigidbody>().AddForce(m_BulletSpawnPoint.forward * (m_BulletSpeed+m_ForwardSpeed), ForceMode.VelocityChange);
        Destroy(bulletGO, m_LifeDuration);
    }

    private void Reset()
    {
        m_NextShootTime = Time.time;

    }

    protected override void GameMenu(GameMenuEvent e)
    {
        Reset();
    }

    protected override void GameOver(GameOverEvent e)
    {
        Destroy(gameObject);
    }


}

