using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{
    public List<Currency> listCurrencies;
    CurrencyPusher currencyPusher;

    public static Action OnMoneyEnter;


    private void Awake()
    {
        currencyPusher = GetComponent<CurrencyPusher>();

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Currency") && !other.GetComponent<Currency>().goToBag && !other.GetComponent<Currency>().isOnTheSafe)
        {
            StartCoroutine(StopMoneyMoving(other.gameObject));
            listCurrencies.Add(other.GetComponent<Currency>());
            other.GetComponent<Currency>().isOnTheSafe = true;
            if(OnMoneyEnter != null)
                OnMoneyEnter();
        }
    }

    //Money içeri girmiþ artýk hareket etmesine gerek yok.
    IEnumerator StopMoneyMoving(GameObject money)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.1f));
        money.GetComponent<Currency>().StopGoingToSafe();
    }
}
