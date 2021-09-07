using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CameraFollow : MonoBehaviour
{
    //The distance between the camera and the object to be followed
    Vector3 Dir;
    //Object to follow
    public GameObject m_Player;
    
    void Start()
    {
        //Get the distance between the camera and the object to be followed
        Dir = m_Player.transform.position - transform.position;
    }

    
    void LateUpdate()
    {
        //Camera position
        transform.position = m_Player.transform.position - Dir;
    }

}