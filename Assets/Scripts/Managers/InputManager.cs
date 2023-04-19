using System.Collections.Generic;
using UnityEngine;
using UnityExtra;

public class InputManager : MonoBehaviour
{
    private static bool isTouching = false;
    private static bool isMoving = false;
    private static bool isFirstTouched = false;

    private void Awake()
    {
        isMoving = false;
        isTouching = false;
        isFirstTouched = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !ExtraCanvasManagement.IsUIObject())
        {
            if (!isFirstTouched)//Swipe To Move yazısını ve cursor u kaldırıyoruz
            {
                isFirstTouched = true;
                GameManager.instance.GameStarted();
            }
            isTouching = true;//Basıp kaldırmaya göre değişiecek
            isMoving = true;//İlk dokunuşta çalışıp birdaha durmayacak
            //Debug.Break();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isTouching = false;
        }
    }

    public static bool IsTouching()
    {
        return isTouching;
    }

    public static bool IsMoving()
    {
        return isMoving;
    }

    public static bool FirstTouched()
    {
        return isFirstTouched;
    }
}
