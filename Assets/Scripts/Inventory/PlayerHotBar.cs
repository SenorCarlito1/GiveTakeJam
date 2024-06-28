using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHotBar : MonoBehaviour
{
    public HotBarObject hotBar;

    private void OnApplicationQuit()
    {
        hotBar.Container.Clear();
    }
}
