using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void Scenes(int sceneEx)
    {
        SceneManager.LoadScene(sceneEx);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
