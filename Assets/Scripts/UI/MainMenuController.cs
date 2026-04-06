using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject creditsPane;
    public GameObject settingsPane;
    
    public void Start()
    {
        creditsPane.SetActive(false);
        settingsPane.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GameRoom");
    }
    public void ToggleSettings()
    {
        if (!settingsPane.activeSelf)
        {
            settingsPane.SetActive(true);
            if (creditsPane.activeSelf) creditsPane.SetActive(false); //Disable credits if theyre currently open
        }
        else settingsPane.SetActive(false);
    }
    public void ToggleCredits()
    {
        if (!creditsPane.activeSelf)
        {
            creditsPane.SetActive(true);
            if (settingsPane.activeSelf) settingsPane.SetActive(false); //Disable settings if theyre currently open
        }
        else creditsPane.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
