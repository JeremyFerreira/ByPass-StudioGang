using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerpositionRef : MonoBehaviour
{
    public LayerMask layer;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(player.position, Vector3.down,out hit))
        {
            transform.position =new Vector3(player.position.x, hit.point.y, player.position.z);
            transform.up = hit.normal;
        }
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        
    }
}
