using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STUDENT_NAME;

public class PlayerController : MonoBehaviour
{
    Rigidbody m_Rigidbody;


    [Header("Translation & Rotation")]
    [Tooltip("Vitesse de translation en m.s-1")]
    [SerializeField] float m_TranslationSpeed;
    [Tooltip("Vitesse de rotation en °.s-1")]
    [SerializeField] float m_RotationSpeed;
    [SerializeField] float m_UprightRotationLerpCoef;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // comportement dynamique cinétique (non-kinematic)
        // Time.fixedDeltaTime
        if (!GameManager.Instance.IsPlaying) return;

        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        Vector3 translationVect = vInput * transform.forward * m_TranslationSpeed * Time.fixedDeltaTime;
        m_Rigidbody.MovePosition(transform.position + translationVect);
        Quaternion qUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion newOrientation = Quaternion.Lerp(transform.rotation, qUpright * transform.rotation, m_UprightRotationLerpCoef * Time.fixedDeltaTime);
        float deltaAngle = hInput * m_RotationSpeed * Time.fixedDeltaTime;
        Quaternion qRot = Quaternion.AngleAxis(deltaAngle, transform.up);

        newOrientation = qRot * newOrientation;

        m_Rigidbody.MoveRotation(newOrientation);

    }
}

