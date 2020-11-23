using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteRotation : MonoBehaviour
{

    public float rotationSpeed = 220f;
    public Vector3 orbDir;
    //public GameObject player;

    //public float orbitDistance = 10.0f;
    //public float orbitDegreesPerSec = 180.0f;

    void Orbit(Vector3 orbitDirection)
    {
       this.transform.RotateAround(this.transform.parent.position, orbitDirection,
                rotationSpeed * Time.deltaTime);
            //transform.RotateAround(target.transform.position, new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);

            // $$anonymous$$eep us at the last known relative position
            //transform.position = target.transform.position + relativeDistance;


            //transform.RotateAround(target.transform.position, new Vector3(0, 0, 1), orbitDegreesPerSec * Time.deltaTime);


            // Reset relative position after rotate
            //relativeDistance = transform.position - target.transform.position;
        }

    void LateUpdate () {

        Orbit(orbDir);

    }

    // Start is called before the first frame update
    void Start()
    {
        orbDir = this.transform.parent.forward;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("TurnOrbit"))
        {
            orbDir = -orbDir;
        }
        //print(target.transform.position);
        //transform.RotateAround(target.transform.position, new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
    }
}
