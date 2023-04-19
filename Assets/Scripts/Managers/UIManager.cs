using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Globalization;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] bool usePlayerPrefs = false;
    [SerializeField] float setMoney = -1;

    public Image currencyImage;
    public List<Sprite> listCurrencyImages;

    [SerializeField] UpgradeButton btnCurrencyRate;
    [SerializeField] UpgradeButton btnPropellerPower;
    [SerializeField] UpgradeButton btnTapImpact;

    //public GameObject menuFailed;
    //public GameObject menuSuccess;

    //public Text textLevel;
    public TextMeshProUGUI moneyCount;

    //public GameObject slideToMove;

    //public Slider progressSlider;


    private void Awake()
    {
        if (instance == null) instance = this;
        MoneyControl();

        if(setMoney >= 0)
            PlayerPrefs.SetFloat("Money", setMoney);
    }

    //private void Start()
    //{
    //    sliderFever.value = 0;
    //    //distance = Vector3.Distance(GameManager.instance.finishTrigger.transform.position, GameManager.instance.player.transform.position);
    //    SetProgressToSlider();


    //    //print("Max Level Index: " + PlayerPrefs.GetInt("MaxLevelIndex"));
    //    //PlayerPrefs.SetInt("MaxLevelIndex", 25);
    //}

    //private void Update()
    //{
    //    if (GameManager.instance.gameIsRunning)
    //    {
    //        SetProgressToSlider();
    //        afterLastRewardSecond += Time.deltaTime;
    //    }
    //}

    public void MoneyControl()
    {
        if (!PlayerPrefs.HasKey("Money"))
            PlayerPrefs.SetFloat("Money", 0);

        moneyCount.text = ToKMB((decimal)PlayerPrefs.GetFloat("Money"));
    }

    public void SetLevelTexts(string currencyRate, float crPrice,string propellerPower, float ppPrice, string tapImpact, float tiPrice,string incDr = "",string incPP = "",string incTI = "")
    {
        btnCurrencyRate.txtValue.text = Math.Round(float.Parse(currencyRate), 2).ToString().Replace(",",".");
        btnCurrencyRate.txtPrice.text = "$"+ToKMB((decimal)crPrice);
        
        btnPropellerPower.txtValue.text = Math.Round(float.Parse(propellerPower), 2).ToString("0.0").Replace(",", ".");
        btnPropellerPower.txtPrice.text = "$" + ToKMB((decimal)ppPrice);

        btnTapImpact.txtValue.text = Math.Round(float.Parse(tapImpact),2).ToString("0.0").Replace(",", ".");
        btnTapImpact.txtPrice.text = "$" + ToKMB((decimal)tiPrice);

        if (!incDr.Equals(""))
            btnCurrencyRate.txtIncreaseValue.text = "+"+incDr.Replace(",", ".");

        if (!incPP.Equals(""))
            btnPropellerPower.txtIncreaseValue.text = "+" + incPP.Replace(",", ".");

        if (!incTI.Equals(""))
            btnTapImpact.txtIncreaseValue.text = "+" + incTI.Replace(",", ".");

    }

    public void ButtonsInteractableControls(float drPrice, float ppPrice,float tiPrice)
    {
        float money = PlayerPrefs.GetFloat("Money");

        if (drPrice > money)
            btnCurrencyRate.DisableButton();
        else if(!btnCurrencyRate.maxLevel)
            btnCurrencyRate.EnableButton();

        if (ppPrice > money)
            btnPropellerPower.DisableButton();
        else if(!btnPropellerPower.maxLevel)
            btnPropellerPower.EnableButton();

        if (tiPrice > money)
            btnTapImpact.DisableButton();
        else if(!btnTapImpact.maxLevel)
            btnTapImpact.EnableButton();
    }


    public void DisableButton(UpgradeButtons upgradeButton)
    {
        if (upgradeButton == UpgradeButtons.CurrencyRate)
            btnCurrencyRate.ReachedMaxLevel();
        else if (upgradeButton == UpgradeButtons.PropellerPower)
            btnPropellerPower.ReachedMaxLevel();
        if (upgradeButton == UpgradeButtons.TapImpact)
            btnTapImpact.ReachedMaxLevel();
    }


    public void OnButtonClick(UpgradeButtons button)
    {
        if(button == UpgradeButtons.CurrencyRate)
            btnCurrencyRate.OnClick();
        else if (button == UpgradeButtons.PropellerPower)
            btnPropellerPower.OnClick();
        else if (button == UpgradeButtons.TapImpact)
            btnTapImpact.OnClick();
    }


    public void ChangeCurrencyUI(int currencyIndex)
    {
        currencyImage.sprite = listCurrencyImages[currencyIndex];
    }



    public void PlayerPrefsControl()
    {
        //Þuaki level indexini alýyoruz
        int levelNowIndex = SceneManager.GetActiveScene().buildIndex;

        //Max level yoksa atama yapýyoruz
        if (!PlayerPrefs.HasKey("MaxLevelIndex")) 
            PlayerPrefs.SetInt("MaxLevelIndex", 0);
        else if (PlayerPrefs.GetInt("MaxLevelIndex") != levelNowIndex && usePlayerPrefs && levelNowIndex == 0)//Eðer maksimum levelde deðilse oraya gitsin
        {
            //Eðer levellerin hepsini oynadýysa random seç
            if (SceneManager.sceneCountInBuildSettings <= PlayerPrefs.GetInt("MaxLevelIndex"))
            {
                RandomLevel();
            }
            else
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("MaxLevelIndex"));
            }
        }

        //Eðer max level 20 yi geçmiþse level kýsmýna onu yaz ve levelNowIndex prefsini deðiþtir
        if (PlayerPrefs.GetInt("MaxLevelIndex") > levelNowIndex)
            PlayerPrefs.SetInt("LevelNowIndex", PlayerPrefs.GetInt("MaxLevelIndex"));
        else
            PlayerPrefs.SetInt("LevelNowIndex", levelNowIndex);

        //textLevel.text = "" + (PlayerPrefs.GetInt("MaxLevelIndex") + 1);


    }

    //Tap To Continue de tap olduktan sonra burasý çaðrýlýyor
    public void NextLevelControl()
    {
        //print("Count: " + SceneManager.sceneCountInBuildSettings + " Max Level Index: " + PlayerPrefs.GetInt("MaxLevelIndex"));
        if (SceneManager.sceneCountInBuildSettings <= PlayerPrefs.GetInt("MaxLevelIndex") + 1)
        {
            //print("Random Level");
            //Random level seçtik ve maxlevelindex i 1 arttýrdýk
            RandomLevel();
            PlayerPrefs.SetInt("MaxLevelIndex", (PlayerPrefs.GetInt("MaxLevelIndex") + 1));
        }
        else
        {
            int y = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("MaxLevelIndex", (y + 1));
            SceneManager.LoadScene(y + 1);
        }
    }


    public static void RandomLevel()
    {
        int y = SceneManager.GetActiveScene().buildIndex;

        int randLevel = UnityEngine.Random.Range(1, SceneManager.sceneCountInBuildSettings);
        while (randLevel == y)
        {
            randLevel = UnityEngine.Random.Range(1, SceneManager.sceneCountInBuildSettings);
        }
        if (randLevel != y)
        {
            SceneManager.LoadScene(randLevel);
        }
    }



    //public void LevelFailed()
    //{
    //    StartCoroutine(ShowMenu(menuFailed, "LevelFailed", 1));
    //}


    //public void LevelSuccess(float waitTime)
    //{
    //    //int nextIndex = PlayerPrefs.GetInt("MaxLevelIndex") + 1;
    //    //PlayerPrefs.SetInt("MaxLevelIndex", nextIndex);
    //    StartCoroutine(ShowMenu(menuSuccess,"LevelSuccess", waitTime));
    //}


    IEnumerator ShowMenu(GameObject menu,string animName,float time)
    {
        yield return new WaitForSeconds(time);

        if (animName == "LevelSuccess")
            menu.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "LEVEL " + (PlayerPrefs.GetInt("MaxLevelIndex") + 1) + "\n COMPLETED";
        else
            menu.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "LEVEL " + (PlayerPrefs.GetInt("MaxLevelIndex") + 1) + "\n FAILED";

        menu.gameObject.SetActive(true);
        menu.GetComponent<Animation>().Play(animName);
    }


    public void IncreaseMoney(float increaseCount)
    {
        PlayerPrefs.SetFloat("Money", PlayerPrefs.GetFloat("Money") + increaseCount);

        string value = ToKMB((decimal)PlayerPrefs.GetFloat("Money"));

        moneyCount.text = value;

    }

    public void DecreaseMoney(float DecreaseCount)
    {
        PlayerPrefs.SetFloat("Money", PlayerPrefs.GetFloat("Money") - DecreaseCount);

        string value = ToKMB((decimal)PlayerPrefs.GetFloat("Money"));

        moneyCount.text = value;

    }



    public string ToKMB(decimal num)
    {
        if (num > 999999999 || num < -999999999)
        {
            return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999999 || num < -999999)
        {
            return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999 || num < -999)
        {
            return num.ToString("0,.#K", CultureInfo.InvariantCulture);
        }
        else
        {
            return num.ToString(CultureInfo.InvariantCulture);
        }
    }

    //void SetProggress()
    //{
    //    float distancenow = vector3.distance(gamemanager.instance.finishtrigger.transform.position, gamemanager.instance.player.transform.position);

    //    progressslider.value = 1.05f - ((distancenow * 100) / distance) / 100;

    //}

    //public float SetProgressToSlider()
    //{
    //    Vector3 finishPos = GameManager.instance.finishTrigger.transform.position;
    //    finishPos.y = GameManager.instance.player.transform.position.y;
    //    progressSlider.value = (100 - Vector3.Distance(finishPos, GameManager.instance.player.transform.position) * 100 / distance) / 100;
    //    return progressSlider.value * 100;
    //}

    //public int GetLevelProgress()
    //{
    //    return (int)SetProgressToSlider();
    //}


}
