using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class devSurveyTest1 : MonoBehaviour
{
    public static devSurveyTest1 _localInstance;

    public devSurvey1Question[] survey;
    public Animator anim;
    public Text questionText;
    public Transform buttonParent;
    public GameObject buttonPrefab;

    public int redScore, yellowScore, blueScore;

    private int currentIndex = 0;
    private List<GameObject> currentButtons = new List<GameObject>();

    private void Awake()
    {
        _localInstance = this;
        UpdateSurveyUI();
    }

    void UpdateSurveyUI()
    {
        for(int i = 0; i < currentButtons.Count; i++)
        {
            Destroy(currentButtons[i]);
        }
        questionText.text = survey[currentIndex].question;
        for(int i = 0; i < survey[currentIndex].answers.Length; i++)
        {
            currentButtons.Add(Instantiate(buttonPrefab, buttonParent));
            currentButtons[currentButtons.Count - 1].GetComponent<devSurvey1Button>().answerData = survey[currentIndex].answers[i];
        }
    }

    public void SelectAnswer(devSurvey1Answer answer)
    {
        if(answer.questionColorType == ColorType.Red)
        {
            redScore += answer.colorWorth;
        }
        else if (answer.questionColorType == ColorType.Yellow)
        {
            yellowScore += answer.colorWorth;
        }
        else if (answer.questionColorType == ColorType.Blue)
        {
            blueScore += answer.colorWorth;
        }

        HahaMoveOnNextQuestion();
    }

    public void HahaMoveOnNextQuestion()
    {
        currentIndex = (currentIndex + 1) % survey.Length;
        anim.SetTrigger("Next");
    }
}

[Serializable]
public class devSurvey1Question
{
    public string question;
    public devSurvey1Answer[] answers;
}

[Serializable]
public class devSurvey1Answer
{
    public string answer;
    public ColorType questionColorType;
    public int colorWorth = 1;
}

public enum ColorType
{
    Red,
    Yellow,
    Blue
}


