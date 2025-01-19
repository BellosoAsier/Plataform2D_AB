using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject imageControls;
    [SerializeField] private GameObject[] menuItems;
    public void PlayButton()
    {
        SceneManager.LoadSceneAsync("01_GameScene");
    }

    public void ControlsButton()
    {
        imageControls.SetActive(true);
        foreach (GameObject go in menuItems)
        {
            go.SetActive(false);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        imageControls.SetActive(false);
        foreach (GameObject go in menuItems)
        {
            go.SetActive(true);
        }
    }
}
