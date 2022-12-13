using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private DataGhost _dataGhost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGhostController (DataGhost dataghost)
    {
        _dataGhost = dataghost;
        gameObject.transform.position = _dataGhost.TransfomPlayer[0];
    }
}
