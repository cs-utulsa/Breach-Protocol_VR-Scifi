using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public GameObject menu;
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }
}
