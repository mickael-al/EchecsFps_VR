using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessVR;
public class Projectile : MonoBehaviour
{
    [SerializeField] private float force = 5.0f;
    [SerializeField] private string ignoreTag = "";
    void Update()
    {
        transform.Translate(Vector3.forward*force*Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {      
        if(other.transform.tag != ignoreTag)
        {  
            if(other.transform.tag == "Piece")
            {
                
            }
            else if(other.GetComponent<Ennemis>())
            {
                other.GetComponent<Ennemis>().addDegat(10);
                Destroy(gameObject);
            }
            else if(other.GetComponent<Player>())
            {
                other.GetComponent<Player>().addDegat(8);
                Destroy(gameObject);
            }
        }
    }
}
