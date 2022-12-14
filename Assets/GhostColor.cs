using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GhostColor : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] TrailRenderer trail;
    [SerializeField] float distanceMin;
    [SerializeField] float distanceMax;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.Instance.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - player.position).magnitude;
        Color color = mat.color;
        float range = (distanceMax - distanceMin);
        color.a = (1- (range - distance +distanceMin) / range);
        if(distance < distanceMin)
        {
            mesh.enabled = false;
            trail.enabled = false;
        }
        else
        {
            mesh.enabled = true;
            trail.enabled = true;
        }
        trail.startColor = color;
        mat.color = color;
    }
}
