using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Vector3 vector;
    // Start is called before the first frame update
    void Start()
    {
        vector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -4)
        {
            transform.position = vector ;
        }
    }
}
