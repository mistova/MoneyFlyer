using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPusher : MonoBehaviour
{
    public static Action OnMoneyPushed;
    
    Safe safe;
    CurrencyCreator creator;

    [Range(0, 20)]
    [SerializeField] float pusherFrequency = 5;
    [SerializeField] float speed = 0.5F;
    [SerializeField] GameObject spherePrfb,target;

    public float tapFrequency = 0.2f;
    public float _tapFrequency = 0;//Basýldýðýnda üstteki deðeri alacak

    Coroutine crtcurrencyPusher;

    //[SerializeField] float angle,heigth;
    private void OnEnable()
    {
        Safe.OnMoneyEnter += StartMoneyPushing;
    }

    private void OnDisable()
    {
        Safe.OnMoneyEnter -= StartMoneyPushing;
    }

    void Start()
    {
        safe = GetComponent<Safe>();
        creator = GetComponent<CurrencyCreator>();
        //StartMoneyPushing();


    }

    public void StartMoneyPushing()
    {
        creator.SetFlowSpeed(pusherFrequency);
        //if (crtcurrencyPusher != null)
        //    StopCoroutine(crtcurrencyPusher);
        if (crtcurrencyPusher == null)
            crtcurrencyPusher = StartCoroutine(IECurrencyPusher());
    }

    public void StartMoneyPushingNow()
    {
        creator.SetFlowSpeed(pusherFrequency);
        if (crtcurrencyPusher != null)
            StopCoroutine(crtcurrencyPusher);

        crtcurrencyPusher = StartCoroutine(IECurrencyPusher());
    }

    IEnumerator IECurrencyPusher()
    {
        //Fazla hýzlandýrýnca bozuluyor çünkü içeride 1 saniyede 15 den veya çok hýzlý gelirlerse 20 den fazla atýþ yapýlamýyor
        while (safe.listCurrencies.Count > 0)
        {
            //yield return new WaitForSeconds(1f / (pusherFrequency + _tapFrequency));
            //PushMoney();

            float length = pusherFrequency + _tapFrequency;
            //print("Length: " + length);
            //print("EverySecond: " + (1f / length));
            //print("_tapFrequency: " + _tapFrequency);
            length = (length < safe.listCurrencies.Count) ? length : safe.listCurrencies.Count;
            //Saniyede 5 tane atmasý lazým
            //for (int i = 0; i < length; i++)
            //{

            //}
            PushMoney();
            yield return new WaitForSeconds(1f / length);
        }
        crtcurrencyPusher = null;
    }

    private void PushMoney()
    {
        int rnd = UnityEngine.Random.Range(0, safe.listCurrencies.Count);

        while (safe.listCurrencies[rnd].goToBag)
        {
            rnd = UnityEngine.Random.Range(0, safe.listCurrencies.Count);
        }

        Currency money = safe.listCurrencies[rnd];
        safe.listCurrencies.RemoveAt(rnd);
        money.GoToBag(target.transform.position, speed);
        OnMoneyPushed();
    }

    public void SetPropellerPower(float value)
    {
        pusherFrequency = value;
    }

    public void SetTapImpact(float value)
    {
        tapFrequency = value;
    }



    public void Tapping()
    {
        _tapFrequency = tapFrequency;
        creator.SetFlowSpeed(pusherFrequency);
        StartMoneyPushingNow();
    }

    public void NotTapping()
    {
        _tapFrequency = 0;
        creator.SetFlowSpeed(pusherFrequency);
    }
}
