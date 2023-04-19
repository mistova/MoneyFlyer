using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExtra;

public class PropellerController : MonoBehaviour
{

    public List<Propeller> listPropellerLevels = new List<Propeller>();
    public List<GameObject> listWingLevels = new List<GameObject>();
    private float _turnTapSpeed = 0;

    [SerializeField] private GameObject pole;
    [SerializeField] private float startTurnSpeed = 100;
    [SerializeField] private float incTurnSpeed = 10;
    [SerializeField] private Vector3 _maxPropellerAnimScale;
    [SerializeField] private float _propellerAnimSpeed;

    public float turnSpeed = 5;
    private List<Propeller> listPropellers = new List<Propeller>();


    int propellerLevelIndex;
    void Awake()
    {
        //foreach (Propeller item in GetComponentsInChildren<Propeller>())
        //    listPropellers.Add(item);

    }

    public void SetStartPropellers(int level)
    {
        //print("level" + level);
        propellerLevelIndex = UpgradeManager.GetPropellerLevelIndex(level);

        int multiply = 50;
        multiply = (propellerLevelIndex > 1) ? 45 : 50;
         
        int wingCount = (level - propellerLevelIndex * multiply);
        int propellerCount = (wingCount / 10) + 1;
        //print("propellerCount" + propellerCount);
        propellerCount = (wingCount % 10 == 0) ? propellerCount - 1 : propellerCount;

        //print("propellerCount" + propellerCount);
        //print("wingCount" + wingCount);
        //print("propellerLevelIndex" + propellerLevelIndex);

        //placeWingCount = (wingCount == 0) ? 2 : wingCount;

        if (propellerLevelIndex > 0)
            AddNewPropeller(10);

        for (int i = 0; i < propellerCount; i++)
        {
            int placeWingCount = (wingCount > 10) ? 10 : wingCount;

            AddNewPropeller(placeWingCount);

            wingCount -= placeWingCount;
        }

    }

    public void SetPropellersLevel(int level)
    {
        turnSpeed = (startTurnSpeed + (level - 1) * incTurnSpeed) + _turnTapSpeed;
        for (int i = 0; i < listPropellers.Count; i++)
        {
            listPropellers[i].SetPropellerLevel(turnSpeed);
        }
    }

    public void AddWings()
    {
        //print("Propeller isFull " + (listPropellers[listPropellers.Count - 1].isFull));

        if(!listPropellers[listPropellers.Count - 1].isFull)
            listPropellers[listPropellers.Count - 1].AddWing(propellerLevelIndex);
        else
        {
            if(listPropellers.Count == 5)
                NewLevelPropeller();
            else
                AddNewPropeller(0);
        }
    }


    private void AddNewPropeller(int wingCount)
    {
        Vector3 polePos = pole.transform.localPosition;
        polePos.y = -0.5f + (listPropellers.Count+1) * 0.5f;
        pole.transform.localPosition = polePos;

        GameObject propeller = Instantiate(listPropellerLevels[propellerLevelIndex].gameObject, transform);
        Vector3 propellerPos = propeller.transform.localPosition = polePos;
        propellerPos.y = -0.25f + listPropellers.Count * 0.5f;
        propeller.transform.localPosition = propellerPos;

        propeller.GetComponent<Propeller>().SetWingCount(wingCount, propellerLevelIndex);
        propeller.GetComponent<Propeller>().MaxPropellerAnimScale = _maxPropellerAnimScale;
        propeller.GetComponent<Propeller>().PropellerAnimSpeed = _propellerAnimSpeed;

        listPropellers.Add(propeller.GetComponent<Propeller>());

    }


    //5 tane propeller yapýlmýþ þimdi daha büyük olana geçicez tekli bir þekilde
    private void NewLevelPropeller()
    {
        for (int i = 0; i < listPropellers.Count; i++)
            listPropellers[i].gameObject.SetActive(false);

        listPropellers.Clear();

        propellerLevelIndex += 1;

        AddNewPropeller(10);
        AddNewPropeller(0);
    }


    public void AddMoreTurnSpeed(float addSpeed)
    {
        //print("Adding: " + addSpeed);
        _turnTapSpeed = addSpeed;
        SetPropellersLevel(PlayerPrefs.GetInt("LvlPropellerPower"));
    }

}
