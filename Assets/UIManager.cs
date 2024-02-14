using TheNecromancers.InputManagers;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] UIInputManager UIInputManager;
    [SerializeField] GameObject inventory = null;
    [SerializeField] GameObject pauseMenu = null;

    private void Awake()
    {
        UIInputManager.OpenInventory += ToggleInventory;
        UIInputManager.OpenPauseMenu += TogglePauseMenu;
    }

    private void OnDestroy()
    {
        UIInputManager.OpenInventory -= ToggleInventory;
        UIInputManager.OpenPauseMenu -= TogglePauseMenu;
    }

    private void Start()
    {
        inventory.SetActive(false);
        pauseMenu.SetActive(false);
    }

    private void ToggleInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
    }

    private void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
}
