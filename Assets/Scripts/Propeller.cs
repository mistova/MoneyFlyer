using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{

    [SerializeField] List<GameObject> listWings;
    public bool isFull = false;

    private PropellerController propellerController;
    private int _wingCount = 2;
    private float _turnSpeed = 5;
    private bool _canAnimate = false;
    private bool _reverseAnim = false;
    private Vector3 _propellerAnimDefScale;
    private Vector3 _maxPropellerAnimScale;
    private float _propellerAnimSpeed = 5;

    public int WingsCount 
    {
        get{ return listWings.Count; }
    }

    public Vector3 MaxPropellerAnimScale
    {
        set { _maxPropellerAnimScale = value; }
    }

    public float PropellerAnimSpeed
    {
        set { _propellerAnimSpeed = value; }
    }

    void Awake()
    {
        propellerController = transform.parent.GetComponent<PropellerController>();
        listWings = new List<GameObject> ();
    }

    public void SetPropellerLevel(float turnSpeed)
    {
        _turnSpeed = turnSpeed;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * propellerController.turnSpeed);

        if (_canAnimate)
        {
            if (!_reverseAnim)
            {
                transform.localScale += Vector3.one * Time.deltaTime * _propellerAnimSpeed;

                if (transform.localScale.x >= _maxPropellerAnimScale.x)
                {
                    _reverseAnim = true;
                }
            }
            else
            {
                transform.localScale -= Vector3.one * Time.deltaTime * _propellerAnimSpeed;
                if (transform.localScale.x <= _propellerAnimDefScale.x)
                {
                        _reverseAnim = false;
                    _canAnimate = false;
                }
            }

        }
    }

    public void SetWingCount(int wingCount,int propellerLevelIndex)
    {
        wingCount = (wingCount < 1) ? 1 : wingCount;
        for (int i = 0; i < wingCount; i++)
        {
            AddWing(propellerLevelIndex);
        }
    }

    public void AddWing(int propellerLevelIndex)
    {
        if (listWings.Count < 10)
        {
            GameObject wingNew = Instantiate(propellerController.listWingLevels[propellerLevelIndex], transform.GetChild(0));
            wingNew.transform.localScale = propellerController.listWingLevels[propellerLevelIndex].transform.localScale;

            listWings.Add(wingNew);

            for (int i = 0; i < listWings.Count; i++)
            {
                GameObject wing = listWings[i];
                wing.transform.eulerAngles = new Vector3(wing.transform.eulerAngles.x, i * 360 / listWings.Count, wing.transform.eulerAngles.z);
            }
        }

        Animate();

        if (listWings.Count >= 10) isFull = true;
    }


    private void Animate()
    {

        if (!_canAnimate)
        {
            _propellerAnimDefScale = transform.localScale;
            _canAnimate = true;
        }
    }

}
