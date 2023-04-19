using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExtra;

public class TapController : MonoBehaviour
{
    public float countUp = 0.5f;

    [SerializeField] private ParticleSystem electric;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private float addTurnSpeed = 300;
    [SerializeField] private float addWindSpeed = 0.025f;

    private Animation animation;
    private bool callNotTappingFunc = false;
    private bool callTappingFunc = false;

    CurrencyPusher currencyPusher;
    PropellerController propellerController;
    WindController windController;

    private bool _sendElectric = false;

    void Awake()
    {
        animation = GetComponent<Animation>();
        windController = FindObjectOfType<WindController>();
        currencyPusher = FindObjectOfType<CurrencyPusher>();
        propellerController = FindObjectOfType<PropellerController>();
        Physics.queriesHitTriggers = true;
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && !ExtraCanvasManagement.IsUIObject())
        //{
        //    if (!callTappingFunc)
        //    {
        //        currencyPusher.Tapping();
        //        propellerController.AddMoreTurnSpeed(addTurnSpeed);
        //        windController.AddMoreWindSpeed(addWindSpeed);
        //        callTappingFunc = true;
        //        callNotTappingFunc = false;
        //    }


        //    countUp = 0.5f;
        //    //Frequency i etkile belirli bir süre
        //}


        if (_sendElectric)
        {
            electric.transform.position = Vector3.MoveTowards(electric.transform.position, endPos, Time.deltaTime * 5);

            if(Vector3.Distance(electric.transform.position,endPos)<= 0.1f)
            {
                SendElectric();
            }
        }

        if(countUp > 0)
            countUp-=Time.deltaTime;
        else
        {
            if (!callNotTappingFunc)
            {
                currencyPusher.NotTapping();
                propellerController.AddMoreTurnSpeed(0);
                windController.AddMoreWindSpeed(0);
                callTappingFunc = false;
                callNotTappingFunc = true;
            }
        }

    }


    private void OnMouseDown()
    {
        if (!callTappingFunc)
        {
            currencyPusher.Tapping();
            propellerController.AddMoreTurnSpeed(addTurnSpeed);
            windController.AddMoreWindSpeed(addWindSpeed);
            callTappingFunc = true;
            callNotTappingFunc = false;
        }
        //StartSendingElectric();

        countUp = 0.5f;

        animation.Play();

    }


    private void StartSendingElectric()
    {
        _sendElectric = true;
        SendElectric();
    }

    private void SendElectric()
    {
        electric.transform.position = startPos;
    }

}
