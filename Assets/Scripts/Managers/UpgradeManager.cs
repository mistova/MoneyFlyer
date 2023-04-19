using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExtra;

public enum UpgradeButtons
{
    CurrencyRate,
    PropellerPower,
    TapImpact
}

public class UpgradeManager : MonoBehaviour
{
    CurrencyCreator currencyCreator;
    CurrencyPusher currencyPusher;
    PropellerController propellerController;
    WindController windController;
    //TapController tapController;

    [Header("Min Prices",order =1)]
    //Min Fiyat kaçtan Baþlayacak
    [SerializeField] int minPriceCurrencyRate = 10000;
    [SerializeField] int minPricePropellerPower = 5000;
    [SerializeField] int minPriceTapImpact = 3000;

    [Header("Increase Ratio", order = 2)]
    //Her Update de kaç artacak
    [SerializeField] float incCurrencyRate = 0.1f;
    [SerializeField] float incPropellerPower = 1;
    [SerializeField] float incTapImpact = 1;

    [Header("Min Values", order = 2)]
    //Min value kaçtan baþlayacak
    [SerializeField] float minCurrencyRate = 0.1f;
    [SerializeField] float minPropellerPower = 1f;
    [SerializeField] float minTapImpact = 1f;

    //Þuanki level
    int lvlCurrencyRate = 0;
    int lvlPropellerPower = 0;
    int lvlTapImpact = 0;

    float crPriceNow = 0;
    float ppPriceNow = 0;
    float tiPriceNow = 0;

    //Currency UI deðiþtirmek için kullanlýyor
    int currencyIndex = 0;

    //Levelin üssünü alýyoruz burdaki rakam ile fiyat artýþý fazla olsun diye
    int priceIncreaseBase = 3;
    int priceIncreaseMultiplier = 1000;

    private void OnEnable()
    {
        Bag.OnMoneyEntered += CallButtonsInteractableControls;
    }

    private void OnDisable()
    {
        Bag.OnMoneyEntered -= CallButtonsInteractableControls;
    }

    private void Awake()
    {
        currencyCreator = FindObjectOfType<CurrencyCreator>();
        currencyPusher = FindObjectOfType<CurrencyPusher>();
        //tapController = FindObjectOfType<TapController>();
        propellerController = FindObjectOfType<PropellerController>();
        windController = FindObjectOfType<WindController>();

    }

    private void Start()
    {
        LevelControl();
        SetLevels();
        SetUpgradeValues();
        currencyCreator.CreateCurrencyOnStart();
        MaxLevelControl(UpgradeButtons.CurrencyRate);
        MaxLevelControl(UpgradeButtons.PropellerPower);
        MaxLevelControl(UpgradeButtons.TapImpact);

        propellerController.SetStartPropellers(PlayerPrefs.GetInt("LvlPropellerPower"));
        windController.SetWindProperties(lvlPropellerPower,GetPropellerLevelIndex(lvlPropellerPower));

    }

    public static int GetPropellerLevelIndex(int lvlPropellerPower)
    {
        int propellerLevelIndex = -1;
        if (ExtraNumberManagement.IsNumberBetween(0, 50, lvlPropellerPower) == 0)
            propellerLevelIndex = 0;
        else if (ExtraNumberManagement.IsNumberBetween(51, 90, lvlPropellerPower) == 0)
            propellerLevelIndex = 1;
        else if (ExtraNumberManagement.IsNumberBetween(91, 150, lvlPropellerPower) == 0)
            propellerLevelIndex = 2;

        return propellerLevelIndex;
    }

    private void LevelControl()
    {
        if (!PlayerPrefs.HasKey("LvlCurrencyRate"))
        {
            PlayerPrefs.SetInt("CurrencyIndex", 0);
            PlayerPrefs.SetInt("LvlCurrencyRate", 1);
            PlayerPrefs.SetInt("LvlPropellerPower", 2);
            PlayerPrefs.SetInt("LvlTapImpact", 1);

            lvlCurrencyRate = PlayerPrefs.GetInt("LvlCurrencyRate");
            lvlPropellerPower = PlayerPrefs.GetInt("LvlPropellerPower");
            lvlTapImpact = PlayerPrefs.GetInt("LvlTapImpact");

            //print("Yaz:"+ PlayerPrefs.GetInt("lvlCurrencyRate"));
        }
        currencyIndex = PlayerPrefs.GetInt("CurrencyIndex");
        UIManager.instance.ChangeCurrencyUI(currencyIndex);
    }


    private void SetLevels()
    {
        lvlCurrencyRate = PlayerPrefs.GetInt("LvlCurrencyRate");
        lvlPropellerPower = PlayerPrefs.GetInt("LvlPropellerPower");
        lvlTapImpact = PlayerPrefs.GetInt("LvlTapImpact");

        int _lvlCurrencyRate = (PlayerPrefs.GetInt("LvlCurrencyRate") - 1) + currencyIndex * 20;
        //print("Level: "+lvlCurrencyRate);
        float currencyRate = minCurrencyRate + _lvlCurrencyRate * incCurrencyRate - (currencyIndex * 100);
        //print("Yaz: " + currencyRate);
        crPriceNow = minPriceCurrencyRate + Mathf.Pow(_lvlCurrencyRate, priceIncreaseBase) * minPriceCurrencyRate;

        int _lvlPropellerPower = PlayerPrefs.GetInt("LvlPropellerPower") - 1;
        float propellerPower = minPropellerPower + _lvlPropellerPower * incPropellerPower;
        ppPriceNow = minPricePropellerPower + Mathf.Pow(_lvlPropellerPower, priceIncreaseBase) * minPricePropellerPower;

        int _lvlTapImpact = PlayerPrefs.GetInt("LvlTapImpact") - 1;
        float tapImpact = minTapImpact + _lvlTapImpact * incTapImpact;
        tiPriceNow = minPriceTapImpact + Mathf.Pow(_lvlTapImpact, priceIncreaseBase) * minPriceTapImpact; //Level Sayýsýnýn karesinin 500 katý ile artsýn

        UIManager.instance.SetLevelTexts(currencyRate.ToString(), crPriceNow, propellerPower.ToString(), ppPriceNow, tapImpact.ToString(), tiPriceNow,
            incCurrencyRate.ToString(), incPropellerPower.ToString(), incTapImpact.ToString());

        CallButtonsInteractableControls();
    }

    private void CallButtonsInteractableControls()
    {
        UIManager.instance.ButtonsInteractableControls(crPriceNow, ppPriceNow, tiPriceNow);
    }


    #region Upgrade

    public void UpgradeCurrencyRate()
    {

        PlayerPrefs.SetInt("LvlCurrencyRate", PlayerPrefs.GetInt("LvlCurrencyRate") + 1);

        SdkManager.instance.Upgraded(0, PlayerPrefs.GetInt("LvlCurrencyRate"));



        //Bir sonraki currency e geçebiliriz 100 oranýna ulaþýlmýþ
        if (PlayerPrefs.GetInt("LvlCurrencyRate") % (100f / incCurrencyRate) == 0 && currencyIndex + 1 != UIManager.instance.listCurrencyImages.Count)
        {
            currencyIndex++;
            PlayerPrefs.SetInt("CurrencyIndex",currencyIndex);
            UIManager.instance.ChangeCurrencyUI(currencyIndex);
            PlayerPrefs.SetInt("LvlCurrencyRate", 1);
            print("Deðiþtir UI Level: " + lvlCurrencyRate);
        }

        SetLevels();
        DecreaseMoneys(0);
        MaxLevelControl(UpgradeButtons.CurrencyRate);
        OnUpgrade(UpgradeButtons.CurrencyRate);
    }

    public void UpgradePropellerPower()
    {
        PlayerPrefs.SetInt("LvlPropellerPower", PlayerPrefs.GetInt("LvlPropellerPower") + 1);

        SdkManager.instance.Upgraded(1, PlayerPrefs.GetInt("LvlPropellerPower"));

        SetLevels();
        DecreaseMoneys(1);
        MaxLevelControl(UpgradeButtons.PropellerPower);
        propellerController.AddWings();
        windController.SetWindProperties(lvlPropellerPower, GetPropellerLevelIndex(lvlPropellerPower));
        currencyPusher.StartMoneyPushing();
        OnUpgrade(UpgradeButtons.PropellerPower);
    }

    public void UpgradeTapImpact()
    {
        PlayerPrefs.SetInt("LvlTapImpact", PlayerPrefs.GetInt("LvlTapImpact") + 1);

        SdkManager.instance.Upgraded(2, PlayerPrefs.GetInt("LvlTapImpact"));

        SetLevels();
        DecreaseMoneys(2);
        MaxLevelControl(UpgradeButtons.TapImpact);
        OnUpgrade(UpgradeButtons.TapImpact);
    }

    public void OnUpgrade(UpgradeButtons button)
    {
        UIManager.instance.OnButtonClick(button);
    }

    #endregion


    private void DecreaseMoneys(int i)
    {
        float _crPriceNow = minPriceCurrencyRate + Mathf.Pow(lvlCurrencyRate - 2, priceIncreaseBase) * minPriceCurrencyRate;
        float _ppPriceNow = minPricePropellerPower + Mathf.Pow(lvlPropellerPower - 2, priceIncreaseBase) * minPricePropellerPower;
        float _tiPriceNow = minPriceTapImpact + Mathf.Pow(lvlTapImpact - 2, priceIncreaseBase) * minPriceTapImpact; //Level Sayýsýnýn karesinin 500 katý ile artsýn

        //Update Yapýlmýþ
        if (i == 0) UIManager.instance.DecreaseMoney(_crPriceNow);
        else if (i == 1) UIManager.instance.DecreaseMoney(_ppPriceNow);
        else if (i == 2) UIManager.instance.DecreaseMoney(_tiPriceNow);

        SetUpgradeValues();
    }

    private void SetUpgradeValues()
    {
        float currencyRate = minCurrencyRate + (lvlCurrencyRate - 1) * incCurrencyRate;
        float propellerPower = minPropellerPower + (lvlPropellerPower-1) * incPropellerPower;
        float tapImpact = minTapImpact + (lvlTapImpact - 1) * incTapImpact;

        //print("Currency Rate: " + currencyRate);
        //print("Currency Index: " + currencyIndex);

        propellerController.SetPropellersLevel(PlayerPrefs.GetInt("LvlPropellerPower"));

        currencyPusher.SetPropellerPower(propellerPower);

        currencyCreator.SetCurrencyRate(currencyRate, currencyIndex);

        currencyPusher.SetTapImpact(tapImpact);

        CallButtonsInteractableControls();
    }


    private void MaxLevelControl(UpgradeButtons val)
    {
        if(val == UpgradeButtons.CurrencyRate)
        {
            float currencyRate = minCurrencyRate + (lvlCurrencyRate - 1) * incCurrencyRate;
            if (currencyIndex + 1 == UIManager.instance.listCurrencyImages.Count && currencyRate >= 100)
            {
                //print("Currency Max Level");
                UIManager.instance.DisableButton(val);
            }
        }
        else if(val == UpgradeButtons.PropellerPower)
        {
            lvlPropellerPower = PlayerPrefs.GetInt("LvlPropellerPower");
            if (lvlPropellerPower >= 130)
                UIManager.instance.DisableButton(val);
        }
        else if (val == UpgradeButtons.TapImpact)
        {
            lvlTapImpact = PlayerPrefs.GetInt("LvlTapImpact") - 1;
            if (lvlTapImpact >= 100)
                UIManager.instance.DisableButton(val);
        }
    }


}
