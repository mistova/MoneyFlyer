using Deform;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public static Action OnMoneyEntered;

    [SerializeField] float animSpeed = 2;

    [SerializeField] GameObject txtEarnedAnimation;

    Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Currency"))
        {
            UIManager.instance.IncreaseMoney(other.GetComponent<Currency>().value);
            StartBagAnimation();
            
            PlayEarnedAnimation(other.GetComponent<Currency>());

            if (OnMoneyEntered != null)
                OnMoneyEntered();
        }
    }

    public void StartBagAnimation()
    {
        //print("Çallll"+crtBagAnim);

        anim.Play();

    }


    public void PlayEarnedAnimation(Currency currency)
    {
        string value = UIManager.instance.ToKMB((decimal)currency.value);
        
        GameObject earned = Instantiate(txtEarnedAnimation,txtEarnedAnimation.transform.parent);

        Vector3 pos = earned.GetComponent<RectTransform>().localPosition;
        pos.x = currency.transform.position.x;
        earned.GetComponent<RectTransform>().localPosition = pos;

        earned.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+"+value;
        earned.transform.GetChild(0).GetComponent<Animation>().Play();

        Destroy(earned, 1);
    }



}
