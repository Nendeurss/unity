using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STUDENT_NAME;

public class PlayerController : MonoBehaviour
{
    Rigidbody m_Rigidbody;


    [Header("Deplacement")]
    [Tooltip("Vitesse en m.s-1")]
    [SerializeField] float m_Speed;
    [Tooltip("Acceleration en m.s-2")]
    [SerializeField] float m_Acceleration;
    [SerializeField] float m_TimeBetweenAcceleration;


    float timeBeforeAcceleration;
    bool canJump;
    //[Tooltip("Vitesse de rotation en °.s-1")]
    //[SerializeField] float m_RotationSpeed;
    //[SerializeField] float m_UprightRotationLerpCoef;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        timeBeforeAcceleration = m_TimeBetweenAcceleration;
        canJump = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //m_Rigidbody.velocity = new Vector3(0, 0, m_Speed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator JumpCouritine()
    {
        m_Rigidbody.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        canJump = false;
        yield return new WaitForSeconds(2);
        canJump = true;
    }

    private void FixedUpdate()
    {
        // comportement dynamique cinétique (non-kinematic)
        // Time.fixedDeltaTime
        if (!GameManager.Instance.IsPlaying) return;
        timeBeforeAcceleration -= Time.deltaTime;

        if (timeBeforeAcceleration < 0)
        {

            m_Speed += m_Acceleration;
            timeBeforeAcceleration = m_TimeBetweenAcceleration;
        }
        //StartCoroutine(PlayerAcceleration());
        //float vInput = Input.GetAxis("Vertical");
        //float hInput = Input.GetAxis("Horizontal");
        Vector3 translationVect = transform.forward * m_Speed * Time.fixedDeltaTime;
        m_Rigidbody.MovePosition(transform.position + translationVect);
        if (canJump && Input.GetKey("space"))
        {
            StartCoroutine(JumpCouritine());
        }

        //Quaternion qUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
        //Quaternion newOrientation = Quaternion.Lerp(transform.rotation, qUpright * transform.rotation, m_UprightRotationLerpCoef * Time.fixedDeltaTime);
        //float deltaAngle = hInput * m_RotationSpeed * Time.fixedDeltaTime;
        //Quaternion qRot = Quaternion.AngleAxis(deltaAngle, transform.up);

        //newOrientation = qRot * newOrientation;

        //m_Rigidbody.MoveRotation(newOrientation);

    }
}

