using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The base class that every Survey Page extends from
public abstract class SurveyPageBase : MonoBehaviour
{
    // The string used to seperate values when returning multiple results.
    public const string seperatorString = ", ";

    // The name to use when returning quiz results. This becomes the column name
    // in an excel sheet.
    public string quizResultName = "???";
    // The current page state
    public SurveyPageState currentPageState;

    // Sets the current page state. Gets called by Client2UIController.
    public abstract void SetPageState(SurveyPageState newPageState);

    // Called every frame while this page is open in the UI.
    // If this method returns true, the UI controller will move on
    // and unload this page / get the page results.
    public abstract bool CheckIfPageCompleted();

    // Returns a QuizResultCollection that indicates what results have
    // been gathered from this page.
    public abstract QuizResultCollection GetPageResults();

    // An overridable method that determines how this page effects the users quiz results.
    // By default it does nothing.
    public virtual QuizAtribute GetQuizEffects()
    {
        return new QuizAtribute(QuizAttributeType.None, 0);
    }

    // An overridable method that takes in the current survey results from the UIController.
    // Gets called by the UIController.
    // By default it does nothing
    public virtual void SendSurveyResults(Dictionary<string, string> surveyResults)
    {
        // does nothing normally.
    }
}

// An enum determining the current state of the page.
public enum SurveyPageState
{
    LoadingIn,
    Displaying,
    LoadingOut
}

// An enum that describes a quiz attribute
// (basically it describes what the quiz is testing for)
public enum QuizAttributeType
{
    None,
    P,      // Predator
    NMZ,    // Nemeziz
    X,      // X
    COPA,   // COPA
}

// A struct that is returned by GetQuizEffects in the base survey page.
// It's used to determine how a page will effect the user's quiz results.
// For example, if a specific answer on a page raises their NMZ score by 2,
// it would have a type of NMZ and a weight of 2.
public struct QuizAtribute
{
    public QuizAttributeType type;
    public int weight;

    public QuizAtribute(QuizAttributeType typeIn, int weightIn)
    {
        type = typeIn;
        weight = weightIn;
    }
}

// Legacy class from the old quiz system.
// A simple class containing a question name, it's weight,
// and it's potential answers.
[System.Serializable]
public class QuizQuestion
{
    public string question;
    public int pointWorth = 1;
    public QuizAnswer[] answers;
}

// Legacy class from the old quiz system.
// A simple class that contains a quiz answer, a sprite to use
// for the answer's button, and the type in which it will
// effect the user's quiz results.
[System.Serializable]
public class QuizAnswer
{
    public string answer;
    public Sprite image;
    public QuizAttributeType answerAttributeType;
}

// A struct that is returned in a collection by each page.
// The purpose of this struct is to hold the column name and
// value for each quiz page result.
public struct QuizPageResult
{
    private const string seperator = ", ";

    public string resultName;
    public string pageResult;



    public QuizPageResult(string nameIn, string resultIn)
    {
        resultName = nameIn;
        pageResult = "\"" + resultIn + "\"";
    }

    public QuizPageResult(string nameIn, string[] results)
    {
        resultName = nameIn;

        pageResult = "\"";
        for(int i = 0; i < results.Length - 1; i++)
        {
            pageResult += results[i] + seperator;
        }
        pageResult += results[results.Length - 1] + "\"";
    }
}

// A struct that is returned by each page.
// It contains a collection of QuizPageResults. The reason this
// has it's own struct is to make the creation of single
// result objects cleaner.
public struct QuizResultCollection
{
    public List<QuizPageResult> list;

    // Creates list with single value.
    public QuizResultCollection(QuizPageResult result)
    {
        list = new List<QuizPageResult>();
        list.Add(result);
    }

    // Creates whole list
    public QuizResultCollection(List<QuizPageResult> results)
    {
        list = results;
    }

    // Creates empty list
    public QuizResultCollection(bool thisDoesNothing)
    {
        list = new List<QuizPageResult>();
        // makes empty list
    }
}


