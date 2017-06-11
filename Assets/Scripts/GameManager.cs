using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    public static GameManager instance = null;
    public static string PlayerName = "", EmailPlayer = "";

    void Awake(){
        if (instance == null){
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public static void loadLoading(){
        SceneManager.LoadScene("Loading");
    }
    public static void loadIntro(){
        SceneManager.LoadScene("Intro");
    }
    public static void loadAboutUs(){
        SceneManager.LoadScene("AboutUs");
    }
    public static void loadTutorials(){
        SceneManager.LoadScene("Tutorials");
    }
    public static void loadLevel(){
        SceneManager.LoadScene("LevelMaze");
    }
    public static void loadNameSelector(){
        SceneManager.LoadScene("NameSelector");
    }
}