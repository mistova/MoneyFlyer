using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    static CurrencyCreator currencyCreator;
    private static List<GameObject> listCurrenciesPrefabs;
    private static Stack<Currency> stackCoins;
    private static Stack<Currency> stackMoneys;
    private static Stack<Currency> stackGolds;
    private static Stack<Currency> stackDiamonds;

    private void Awake()
    {
        currencyCreator = FindObjectOfType<CurrencyCreator>();
        stackCoins = new Stack<Currency>();
        stackMoneys = new Stack<Currency>();
        stackGolds = new Stack<Currency>();
        stackDiamonds = new Stack<Currency>();
    }


    public static Currency PopCurrency(Currencies currencyType)
    {
        Currency currency = null;

        if(currencyType == Currencies.Coin)
        {
            if (stackCoins.Count > 0)
            {
                currency = stackCoins.Pop();
            }
            else
            {
                currency = Instantiate(currencyCreator.listCurrenciesPrefabs[0].gameObject).GetComponent<Currency>();
                Debug.LogWarning("Currency Created: " + currencyType);
            }
        }
        else if (currencyType == Currencies.Money)
        {
            if (stackMoneys.Count > 0)
            {
                currency = stackMoneys.Pop();
            }
            else
            {
                currency = Instantiate(currencyCreator.listCurrenciesPrefabs[1].gameObject).GetComponent<Currency>();
                Debug.LogWarning("Currency Created: " + currencyType);
            }
        }
        else if (currencyType == Currencies.Gold)
        {
            if (stackGolds.Count > 0)
            {
                currency = stackGolds.Pop();
            }
            else
            {
                currency = Instantiate(currencyCreator.listCurrenciesPrefabs[2].gameObject).GetComponent<Currency>();
                Debug.LogWarning("Currency Created: " + currencyType);
            }
        }
        else if (currencyType == Currencies.Diamond)
        {
            if (stackDiamonds.Count > 0)
            {
                currency = stackDiamonds.Pop();
            }
            else
            {
                currency = Instantiate(currencyCreator.listCurrenciesPrefabs[3].gameObject).GetComponent<Currency>();
                Debug.LogWarning("Currency Created: " + currencyType);
            }
        }

        currency.isOnTheSafe = false;
        currency.goToBag = false;
        currency.gameObject.SetActive(true);
        currency.GetComponent<Rigidbody>().isKinematic = false;

        return currency;
    }



    public static void PushCurrency(Currency currency)
    {
        //print("Currency Pushed: " + currency.currencyType);

        if (currency.currencyType == Currencies.Coin)
        {
            stackCoins.Push(currency);
        }
        else if (currency.currencyType == Currencies.Money)
        {
            stackMoneys.Push(currency);
        }
        else if (currency.currencyType == Currencies.Gold)
        {
            stackGolds.Push(currency);
        }
        else if (currency.currencyType == Currencies.Diamond)
        {
            stackDiamonds.Push(currency);
        }

    }


}
