using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    bool canRotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        int a = this.transform.childCount;
        canRotate = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire2") && canRotate)
        {
            //plane0.Rotate(new Vector3(0, 0, 1),90);
            //plane1.Rotate(new Vector3(0, 0, 1), 90);
            //plane2.Rotate(new Vector3(0, 0, 1), 90);
            //plane3.Rotate(new Vector3(0, 0, 1), 90);
            this.transform.Rotate(new Vector3(0, 0, 1), 90);
            StartCoroutine(RotateCoroutine());
        }
    }

    IEnumerator RotateCoroutine()
    {
        
        canRotate = false;
        yield return new WaitForSeconds(1f);
        canRotate = true;
    }
}
