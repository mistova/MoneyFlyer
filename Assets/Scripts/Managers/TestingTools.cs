#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestingTools : MonoBehaviour
{
    InputField goLevel;

    private void Start()
    {
        CreateButton("Level Finished", LevelFinished, new Vector2(-200, 400), new Vector2(150, 40));
        //CreateButton("Prefs Delete", PlayerPrefsDelete, new Vector2(-75, 300), new Vector2(200, 50));
        //goLevel = CreateInputField("Go Level", new Vector2(-105, 200), new Vector2(40, 30));
        //CreateButton("Go", GoLevel, new Vector2(-45, 100), new Vector2(200, 50));
    }


    void CreateButton(string name, Action _method, Vector2 pos, Vector2 size)
    {
        GameObject button = new GameObject(name);
        button.AddComponent<RectTransform>();
        button.AddComponent<Image>();
        button.AddComponent<Button>();
        button.transform.parent = UIManager.instance.transform;
        button.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
        button.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        button.GetComponent<RectTransform>().anchoredPosition = pos;
        button.GetComponent<RectTransform>().sizeDelta = size;
        button.GetComponent<Button>().onClick.AddListener(_method.Invoke);

        GameObject text = new GameObject("Text");
        text.AddComponent<RectTransform>();
        text.AddComponent<Text>();
        text.GetComponent<Text>().text = name;
        text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.GetComponent<Text>().fontSize = 45;
        text.GetComponent<Text>().color = Color.black;
        text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        text.transform.parent = button.transform;
        text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        text.GetComponent<RectTransform>().sizeDelta = size;
        text.GetComponent<RectTransform>().localScale = new Vector2(0.4f, 0.4f);

    }

    InputField CreateInputField(string name, Vector2 pos, Vector2 size)
    {
        GameObject input = new GameObject("Go Level");
        input.AddComponent<RectTransform>();
        input.AddComponent<Image>();
        input.AddComponent<InputField>();
        input.transform.parent = UIManager.instance.transform;
        input.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
        input.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        input.GetComponent<RectTransform>().anchoredPosition = pos;
        input.GetComponent<RectTransform>().sizeDelta = size;

        GameObject text = new GameObject("Text");
        text.AddComponent<RectTransform>();
        text.AddComponent<Text>();
        text.GetComponent<Text>().text = "...";
        text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.GetComponent<Text>().color = Color.black;
        text.transform.parent = input.transform;
        text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        text.GetComponent<RectTransform>().sizeDelta = size;
        text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        input.GetComponent<InputField>().textComponent = text.GetComponent<Text>();

        return input.GetComponent<InputField>();
    }
    public void LevelFinished()
    {
        //UIManager.instance.LevelSuccess(0);
        GameManager.instance.tapToContinue = true;
    }

    public void PlayerPrefsDelete()
    {
        PlayerPrefs.DeleteAll();
    }

    public void GoLevel()
    {
        SceneManager.LoadScene(Int32.Parse(goLevel.text) - 1);
    }

}
#endif
