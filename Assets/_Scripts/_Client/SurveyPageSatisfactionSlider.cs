using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyPageSatisfactionSlider : SurveyPageSliders
{
    public override QuizResultCollection GetPageResults()
    {
        return new QuizResultCollection(new QuizPageResult("adidas recommendation score", pageSliders[0].value.ToString()));
    }
}
