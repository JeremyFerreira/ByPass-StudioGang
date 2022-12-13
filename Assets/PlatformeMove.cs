using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformeMove : MonoBehaviour
{
    [SerializeField] enum MovementType { horizontal, rotate, vertical};
    [SerializeField] MovementType type;
    [SerializeField] float length, speed;
    public float timer = 1;
    float timerReset;
    public Transform platform;
    bool goLeft;
    // Start is called before the first frame update
    void Start()
    {
        timerReset = timer;
        platform.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime*speed;
        if(type == MovementType.horizontal)
        {
            MoveHorizontal();
        }
    }
    public void MoveHorizontal()
    {
        if(timer <= 0)
        {
            goLeft = !goLeft;
            timer = timerReset;
        }
        if(goLeft)
        {
            platform.position = Vector3.Lerp(transform.position - transform.right * length, transform.position + transform.right * length, 1 - (timerReset - timer) / timerReset);
        }
        else
        {
            platform.position = Vector3.Lerp(transform.position + transform.right * length , transform.position - transform.right * length, 1 - (timerReset - timer) / timerReset);
        }
        
    }
}
