using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyCreator : MonoBehaviour
{
    Safe safe;
    public List<Currency> listCurrenciesPrefabs;
    [SerializeField] List<Transform> listStartPosses;

    [SerializeField] float currencyFlowSpeed = 2;
    [SerializeField] float startCurrencyCount = 24;

    public float currencyRate = 0.01f;
    public int currencyIndex =0 ;

    float _currencyFlowSpeed = 2;

    bool canCreateMoney = false;
    int rndPos = 0;

    void Start()
    {
        safe = FindObjectOfType<Safe>();
        _currencyFlowSpeed = currencyFlowSpeed;

        //Invoke("CreateCurrency", 1);
        //StartCoroutine(StartCurrencyCreator());
        //CreateCurrencyOnStart();
    }


    private void OnEnable()
    {
        CurrencyPusher.OnMoneyPushed += StartCreating;
    }

    private void OnDisable()
    {
        CurrencyPusher.OnMoneyPushed -= StartCreating;
    }

    public void SetFlowSpeed(float frequency)
    {
        currencyFlowSpeed = _currencyFlowSpeed + frequency * 0.35f;
    }

    public void StartCreating()
    {
        canCreateMoney = true;
        float rnd = Random.Range(0f, 100f);
        Currency crcNew = null;
        //print(currencyIndex);

        int temp = (currencyIndex + 1 >= listCurrenciesPrefabs.Count) ? listCurrenciesPrefabs.Count - 1 : currencyIndex;

        //print("currencyRate: " + currencyRate);
        //print("rnd: " + rnd);

        if (rnd <= currencyRate)
            crcNew = listCurrenciesPrefabs[temp + 1];
        else
            crcNew = listCurrenciesPrefabs[temp];
        
        Currency currency = PoolingManager.PopCurrency(crcNew.currencyType);
        currency.transform.position = listStartPosses[rndPos].transform.position;
        currency.transform.rotation = listStartPosses[rndPos].transform.rotation;
        currency.GetComponent<Currency>().GoToSafe(currencyFlowSpeed);
        rndPos++;
        rndPos = (rndPos == 2) ? 0 : rndPos;
        //StartCoroutine(IECreateMoney());
    }


    public void CreateCurrencyOnStart()
    {
        //print("currencyIndex: " + currencyIndex);

        canCreateMoney = true;
        float rnd = Random.Range(0f, 100f);
        GameObject crcNew = null;

        //int temp = (currencyIndex + 1 >= listCurrenciesPrefabs.Count) ? listCurrenciesPrefabs.Count : currencyIndex + 1;
        //print("Temp: " + temp);
        crcNew = listCurrenciesPrefabs[currencyIndex].gameObject;
        //if (rnd <= currencyRate) crcNew = listCurrenciesPrefabs[temp].gameObject;
        //else crcNew = listCurrenciesPrefabs[temp - 1].gameObject;

        int length = safe.listCurrencies.Count;
        List<Transform> list = new List<Transform>();

        if(currencyIndex > 0)
        {
            for (int i = 0; i < length; i++)
            {
                list.Add(safe.listCurrencies[i].transform);
                GameObject currency = Instantiate(crcNew, list[i].transform.position, list[i].transform.rotation);
                safe.listCurrencies[i].gameObject.SetActive(false);
            }
            safe.listCurrencies.Clear();

        }

    }


    public void SetCurrencyRate(float value,int currencyIndex)
    {
        currencyRate = value;
        this.currencyIndex = currencyIndex;
    }
}
