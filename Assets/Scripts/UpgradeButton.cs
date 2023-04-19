using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    Animation animation;
    public bool maxLevel = false;
    public Image imgDisable;
    public TextMeshProUGUI txtValue;
    public TextMeshProUGUI txtPrice;
    public TextMeshProUGUI txtIncreaseValue;

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    public void DisableButton()
    {
        GetComponent<Button>().interactable = false;
        imgDisable.enabled = true;
    }

    public void EnableButton()
    {
        GetComponent<Button>().interactable = true;
        imgDisable.enabled = false;
    }

    public void ReachedMaxLevel()
    {
        maxLevel = true;
        GetComponent<Button>().interactable = false;
        imgDisable.enabled = true;
        imgDisable.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void OnClick()
    {
        if (animation.isPlaying)
            animation.Stop();

        animation.Play();
    }
}
