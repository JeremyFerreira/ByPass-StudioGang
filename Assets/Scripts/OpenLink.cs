using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    [SerializeField] string _UrlToOpen;

    public void OnClick()
    {
        Application.OpenURL(_UrlToOpen);
    }
}
