using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetJumpRing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.SetCanDoubleJump(true);
        }
    }
}
