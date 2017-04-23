using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsLoader : MonoBehaviour {

    public static void loadLoading()
    {
        SceneManager.LoadScene("Loading");
    }
    public static void loadIntro()
    {
        SceneManager.LoadScene("Intro");
    }
    public static void loadControls()
    {
        SceneManager.LoadScene("Controls");
    }
    public static void loadTutorials()
    {
        SceneManager.LoadScene("Tutorials");
    }
    public static void loadLevel1()
    {
        SceneManager.LoadScene("Level1_Maze");
    }
}
