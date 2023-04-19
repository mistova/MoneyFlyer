using UnityEngine;

public class SdkManager : MonoBehaviour
{
    public static SdkManager instance;

    int currentLevel;

    void Start()
    {
        if (instance == null)
            instance = this;

        LevelStarted();
    }

    public void LevelStarted()
    {
    }

    internal void LevelFinished(bool success)
    {

    }

    internal void Upgraded(int index,int level)
    {

    }

}