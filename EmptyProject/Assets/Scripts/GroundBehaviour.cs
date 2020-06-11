using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using STUDENT_NAME;

public class GroundBehaviour : SimpleGameStateObserver
{
    Material m_GroundMaterial;
    Transform m_Transform;
    //Rigidbody m_Rigidbody;



    protected override void Awake()
    {
        base.Awake();
        //m_Rigidbody = GetComponent<Rigidbody>();

        m_GroundMaterial = GetComponent<Material>();
        m_Transform = transform;
        transform.position = new Vector3(0, 0, transform.localScale.z * 5 - 1);
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
 
    }
}
