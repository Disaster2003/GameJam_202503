using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Vector3 vector;
    Vector3 move;
    public float speed;
    public bool isf;
    // Start is called before the first frame update
    void Start()
    {
        vector = transform.position;
        move = new Vector3(0,-1,1);
    }

    // Update is called once per frame
    void Update()
    {
        if(isf)
        {
            transform.position += move * speed * Time.deltaTime;
        }
        else
        {
            transform.position -= move * speed * Time.deltaTime;
        }
    }
}
