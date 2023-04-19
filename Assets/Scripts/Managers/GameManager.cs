using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool gameIsRunning = true;
    public bool levelSuccess = false;
    public bool tapToContinue = false;


    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && tapToContinue && !UnityExtra.ExtraCanvasManagement.IsUIObject())
        {
            UIManager.instance.NextLevelControl();
        }
    }


    public void GameStarted()
    {
        //UIManager.instance.slideToMove.SetActive(false);
    }

    //public void LevelSuccess()
    //{
    //    levelSuccess = true;
    //    gameIsRunning = false;
    //    UIManager.instance.LevelSuccess(2);

    //    ParticleManager.instance.Play(ParticleManager.instance.PS_Confetti_1, true);

    //    tapToContinue = true;
    //}




    public static float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }





}
