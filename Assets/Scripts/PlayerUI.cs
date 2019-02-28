using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform fuelFill;
    [SerializeField]
    GameObject pauseMenu;

    private PlayerController controller;
    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }

    void Start()
    {
        PauseMenu.IsOn = false;
    }
    void Update()
    {
        SetFuelAmount(controller.GetFuelAmount());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;

    }
    void SetFuelAmount(float _amount)
    {
        fuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
