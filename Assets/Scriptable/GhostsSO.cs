using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ghosts", menuName = "GhostSO/Ghosts", order = 1)]
public class GhostsSO : ScriptableObject
{
    public List<DataGhost> ghostsToShow;
}
