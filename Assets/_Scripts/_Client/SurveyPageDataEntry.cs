using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Mail;
using System;

public class SurveyPageDataEntry : SurveyPagePulsateContinueBase
{
    [Header("Manually Assigned Required References")]
    // Reference to the name input field
    public InputField nameField;
    // Reference to the email input field
    public InputField emailField;
    // Reference to the shoe size dropdown box
    public Dropdown shoeSizeDropdown;
    // Reference to the gender dropdown box
    public Dropdown genderDropdown;
    // References to the birth month, day, and year dropdown boxes.
    // These are controlled & managed slightly differently than the others.
    public Dropdown birthMonth, birthDay, birthYear;
    // Reference to the "i've read the legal stuff" checkbox.
    public Toggle legalToggle;
    // Reference to the continue button
    public Button continueButton;
    // Reference to the images that will display if an email has been entered
    // either correctly or incorrectly.
    public Image emailValidIndicator, emailInvalidIndicator;
    // The color used on the dropdown box text when it's default value is selected
    public Color dropdownUnchangedColor;

    // Stores if the page is completed. Each time the continue button
    // is pressed, it assigns this to the value of CanContinue().
    private bool hasHitContinue = false;
    // Stores if the user has entered a valid email. Assigned to the value
    // of VerifyEmailField() every frame.
    private bool emailIsValid = false;
    // A list of arrays that contains the days in each month. It's used by
    // the date selection system. For example, days[1] contains an array
    // of Dropdown.OptionDatas, with each one being that day of the month.
    private List<Dropdown.OptionData>[] daysInEachMonth;

    private void Awake()
    {
        continueButton.onClick.AddListener(ButtonHitContinue);
        SetupBirthdayPicker();
    }

    private new void Update()
    {
        bool interactable = currentPageState == SurveyPageState.Displaying;

        continueButton.interactable = interactable;
        nameField.interactable = interactable;
        emailField.interactable = interactable;
        birthMonth.interactable = interactable;
        birthDay.interactable = birthMonth.interactable && birthMonth.value != 0;
        birthYear.interactable = birthDay.interactable && birthDay.value != 0;
        shoeSizeDropdown.interactable = interactable;
        genderDropdown.interactable = interactable;
        legalToggle.interactable = interactable;

        emailIsValid = VerifyEmailField();
        emailValidIndicator.enabled = emailIsValid;
        emailInvalidIndicator.enabled = !emailIsValid;

        if(daysInEachMonth != null)
        {
            birthDay.options = daysInEachMonth[birthMonth.value];
        }

        // Set Color of dropdown box text
        genderDropdown.captionText.color = (genderDropdown.value != 0) ? Color.white : dropdownUnchangedColor;
        shoeSizeDropdown.captionText.color = (shoeSizeDropdown.value != 0) ? Color.white : dropdownUnchangedColor;
        birthMonth.captionText.color = (birthMonth.value != 0) ? Color.white : dropdownUnchangedColor;
        birthDay.captionText.color = (birthDay.value != 0) ? Color.white : dropdownUnchangedColor;
        birthYear.captionText.color = (birthYear.value != 0) ? Color.white : dropdownUnchangedColor;

        base.Update();
    }

    // Called by a button in the page's UI
    public void ButtonHitContinue()
    {
        hasHitContinue = CanContinue();
    }

    // Called by ButtonHitContinue() & CheckIfCanContinue()
    // Determines if the user has completed everything they need to on the page
    private bool CanContinue()
    {
        bool output = true;
        output = output && !string.IsNullOrEmpty(nameField.text.Trim());
        output = output && emailIsValid;
        output = output && birthDay.value != 0;
        output = output && birthMonth.value != 0;
        output = output && birthYear.value != 0;
        output = output && shoeSizeDropdown.value != 0;
        output = output && genderDropdown.value != 0;

        output = output && legalToggle.isOn;

        return output;
    }

    // Called by Client2UIController.
    // When this returns true, the UI controller will considered the page
    // completed and move on.
    public override bool CheckIfPageCompleted()
    {
        return hasHitContinue;
    }

    // Returns multiple quiz results including:
    //      email
    //      name
    //      birthday
    //      gender
    //      shoe size
    public override QuizResultCollection GetPageResults()
    {
        List<QuizPageResult> output = new List<QuizPageResult>();

        output.Add(new QuizPageResult("email", emailField.text));
        output.Add(new QuizPageResult("name", nameField.text.ToLower()));
        output.Add(new QuizPageResult("birthday", GenerateBirthdayString()));
        output.Add(new QuizPageResult("gender", genderDropdown.options[genderDropdown.value].text.ToLower()));
        output.Add(new QuizPageResult("shoe size", shoeSizeDropdown.options[shoeSizeDropdown.value].text));

        return new QuizResultCollection(output);
    }

    public override void SetPageState(SurveyPageState newPageState)
    {
        currentPageState = newPageState;
    }

    // Verifies that the email in emailField.text is a valid email
    // according to the .NET MailAddress standards.
    //
    // returns true if the email is valid, false otherwise.
    private bool VerifyEmailField()
    {
        bool isValid = true;

        string userAddress;
        try
        {
            userAddress = new MailAddress(emailField.text).Address;
        }
        catch(System.Exception ex)
        {
            if(ex is System.FormatException || ex is System.ArgumentException)
            {
                isValid = false;
            }
            else
            {
                throw;
            }
        }


        return isValid;
    }

    // Called by base class, SurveyPagePulsateContinueBase.
    // Determines if the user can finish the page
    public override bool CheckIfCanContinue()
    {
        return CanContinue();
    }

    // Sets up the Birth day, month, and year dropdown boxes.
    // Called in Awake()
    private void SetupBirthdayPicker()
    {
        string[] months = {"DATE OF BIRTH...", "January", "February", "March", "April", "May",
            "June", "July", "August", "September", "October", "November", "December"};
        int[] daysPerMonth = { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        List<Dropdown.OptionData> monthList = new List<Dropdown.OptionData>();
        List<Dropdown.OptionData>[] monthDay = new List<Dropdown.OptionData>[months.Length];
        for (int i = 0; i < months.Length; i++)
        {
            monthList.Add(new Dropdown.OptionData(months[i]));

            monthDay[i] = new List<Dropdown.OptionData>();
            monthDay[i].Add(new Dropdown.OptionData("DAY..."));
            for(int x = 0; x < daysPerMonth[i]; x++)
            {
                monthDay[i].Add(new Dropdown.OptionData((x + 1).ToString()));
            }
        }

        List<Dropdown.OptionData> yearList = new List<Dropdown.OptionData>();
        int currentYear = 2018;
        for(int i = currentYear; i >= 1900; i--)
        {
            yearList.Add(new Dropdown.OptionData(i.ToString()));
        }

        birthMonth.options = monthList;
        birthYear.AddOptions(yearList);
        daysInEachMonth = monthDay;
    }

    // Turns the date that the user entered into
    // a readable date/time string following
    // the format MM-dd-yyyy
    private string GenerateBirthdayString()
    {
        if (birthYear.value == 0 || birthMonth.value == 0 || birthDay.value == 0)
        {
            return string.Format("ERR {0}-{1}-{2}", birthMonth.value, birthDay.options[birthDay.value].text, birthYear.options[birthYear.value].text);
        }

        int parsedYear = -1;
        int.TryParse(birthYear.options[birthYear.value].text, out parsedYear);


        DateTime dt = new DateTime(parsedYear, birthMonth.value, birthDay.value);
        return dt.ToString("MM-dd-yyyy");
    }
}

