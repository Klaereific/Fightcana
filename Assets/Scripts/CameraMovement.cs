using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;

    public float outerBounds = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        float mean_x = (player1.transform.position.x + player2.transform.position.x) / 2;
        //transform.position.x = mean_x;
    }

    // Update is called once per frame
    void Update()
    {
        float mean_x = (player1.transform.position.x + player2.transform.position.x) / 2;
        //transform.position.x = mean_x;
    }
}
