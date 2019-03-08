using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform fuelFill;
    [SerializeField]
    RectTransform helathBar;
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject scoreboard;
    [SerializeField]
    Text ammoText;

    private Player player;
    private PlayerController controller;
    private WeaponManager weaponManager;

    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<WeaponManager>();
    }
    
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

        SetHealthAmount(player.GetHealthPct());
        SetAmmoAmmount(weaponManager.GetCurrentWeapon().bullets);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;

    }
    void SetFuelAmount(float _amount)
    {
        fuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
    void SetHealthAmount(float amount)
    {
        helathBar.localScale = new Vector3(1f, amount, 1f);
    }
    void SetAmmoAmmount(int _amount)
    {
        ammoText.text = _amount.ToString();
    }
}
