
using UnityEngine;

public class PlatformeMove : MonoBehaviour
{
    [SerializeField] enum MovementType { horizontal, vertical, rotation};
    [SerializeField] MovementType type;
    [SerializeField] float length, speed;
    public float timer = 1;
    float timerReset;
    public Transform platform;
    bool goLeft;

    // Start is called before the first frame update
    void Start()
    {
        timerReset = 1;
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
        if (type == MovementType.vertical)
        {
            MoveVertical();
        }
        if (type == MovementType.rotation)
        {
            Rotate();
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
    public void MoveVertical()
    {
        if (timer <= 0)
        {
            goLeft = !goLeft;
            timer = timerReset;
        }
        if (goLeft)
        {
            platform.position = Vector3.Lerp(transform.position - transform.up * length, transform.position + transform.up * length, 1 - (timerReset - timer) / timerReset);
        }
        else
        {
            platform.position = Vector3.Lerp(transform.position + transform.up * length, transform.position - transform.up * length, 1 - (timerReset - timer) / timerReset);
        }

    }
    public void Rotate()
    {
        platform.Rotate(new Vector3(0, 1, 0) * speed * Time.deltaTime, Space.Self);
    }
}
