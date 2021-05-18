using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPlay : MonoBehaviour
{
    public MenuManager menuManager;


    public void OnMouseOver()
    {
            if (Input.GetMouseButtonDown(0))
            {
                menuManager.PlayGame();
            }

    }


}
