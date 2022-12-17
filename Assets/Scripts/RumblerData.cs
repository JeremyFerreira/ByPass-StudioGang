using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Rumbler", menuName = "RumblerSO/rumble" )]
public class RumblerData : ScriptableObject
{
    public enum RumblePattern
    {
        Constant,
        Pulse,
        Linear
    }
    public RumblePattern activeRumbePattern;
    public float rumbleDurration;
    public float pulseDurration;
    public float lowA;
    public float lowStep;
    public float highA;
    public float highStep;
    public float rumbleStep;
    public float burstTime;
}
