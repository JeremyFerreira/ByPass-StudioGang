using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GhostSO", menuName = "GhostSO", order = 1)]
public class GhostSO : ScriptableObject
{
    [field: SerializeField]public DataGhost Ghost { get; set; }
    public delegate void OnValueChange();
    public OnValueChange ValueChange;

}
