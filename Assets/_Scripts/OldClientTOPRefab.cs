using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldClientTOPRefab : MonoBehaviour
{
    public ClientSurvey clientSurvey;

    public GameObject quizQuestionPrefab;

    public Transform spawnTransform;

    private void Awake()
    {
        ToPrefab();
    }

    private void ToPrefab()
    {
        for(int i = 0; i < clientSurvey.survey.Length; i++)
        {
            GameObject newObj = Instantiate(quizQuestionPrefab, spawnTransform);
            newObj.name = "QuizQuestion " + clientSurvey.survey[i].question;
            SurveyPageQuizQuestion page = newObj.GetComponent<SurveyPageQuizQuestion>();
            
            page.surveyQuestion.answers = new QuizAnswer[clientSurvey.survey[i].answers.Length];
            for(int x = 0; x < clientSurvey.survey[i].answers.Length; x++)
            {
                page.surveyQuestion.answers[x] = new QuizAnswer();
                page.surveyQuestion.answers[x].answer = clientSurvey.survey[i].answers[x].answer;
                page.surveyQuestion.answers[x].answerAttributeType = (QuizAttributeType)((int)clientSurvey.survey[i].answers[x].answerShoeType + 1);
                page.surveyQuestion.answers[x].image = clientSurvey.survey[i].answers[x].image;
            }

            page.surveyQuestion.pointWorth = clientSurvey.survey[i].pointWorth;
            page.surveyQuestion.question = clientSurvey.survey[i].question;
        }
    }
}
