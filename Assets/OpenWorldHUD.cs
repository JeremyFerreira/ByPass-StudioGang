using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWorldHUD : MonoBehaviour
{
    HudManager hudManager;
    [SerializeField] int WorldDataIndex;
    private void Awake()
    {
        hudManager = GetComponentInParent<HudManager>();    
    }

    public void OnClick()
    {
        hudManager.OpenLevelSelector(WorldDataIndex);
    }
}
