using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyPageSatisfactionSlider2 : SurveyPageSliders
{
    public override QuizResultCollection GetPageResults()
    {
        List<QuizPageResult> results = new List<QuizPageResult>();
        results.Add(new QuizPageResult("adidas recommendation score", pageSliders[0].value.ToString()));
        results.Add(new QuizPageResult("dicks sporting goods recommendation score", pageSliders[1].value.ToString()));
        return new QuizResultCollection(results);
    }
}
