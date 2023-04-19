using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenScene : MonoBehaviour
{
    public static bool cheatBool;

    bool lost = false;
    
    private void Start()
    {
        cheatBool = false;

    }
    public void openScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public static void _openScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }


    public void Restart()
    {
        int y = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(y);
    }

    public void NextLevel()
    {
        int y = SceneManager.GetActiveScene().buildIndex;
        if(y+1 == 6) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(y+1);
    }
    public static void _NextLevel()
    {
        int y = SceneManager.GetActiveScene().buildIndex;
        if (y + 1 == 6) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(y + 1);
    }


    public void Lost()
    {
        if (!lost)//İki defa oynatmasın diye
        {
            StartCoroutine(WaitForIt("Lost", 2));
            lost = true;
        }
    }

    public void Win()
    {
        StartCoroutine(WaitForIt("Win", 5));
    }

    public IEnumerator WaitForIt(string what, float wait)
    {
        yield return new WaitForSeconds(wait);

        if (what == "Lost")
        {

        }
        else if (what == "Win")
        {

            GameObject.Find("Congratulations").GetComponent<Image>().enabled = false;

        }
    }

    //public void ShowFinishUI()
    //{
    //    General.instance.finishUI.SetActive(true);
    //    General.instance.isGameRunning = false;
    //}

    //public void ResumeGame()
    //{
    //    General.instance.finishUI.SetActive(false);
    //    General.instance.isGameRunning = true;
    //}
}
