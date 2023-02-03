using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Menu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;


    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        CloseMenu(mainMenu);
        CloseMenu(settingsMenu);
    }
    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }
    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }


    public void Save()
    {
        Debug.Log("Save Command in Settings not setup ");
        // Game Data Save Logic
    }

}
