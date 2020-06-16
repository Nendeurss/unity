using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STUDENT_NAME;


public class BulletBehaviour : SimpleGameStateObserver
{
    Rigidbody m_Rigidbody;
    Transform m_Transform;

    [Header("Duree de vie")]
    [SerializeField]
    private float m_LifeDuration;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }

    private void Reset()
    {
        Destroy(gameObject);

    }

    protected override void GameMenu(GameMenuEvent e)
    {
        Reset();
    }

}
