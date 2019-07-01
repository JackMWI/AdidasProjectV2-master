using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Form
{
    public string name, email;
    public string cleat;
    public string gender;
    public int shoe;

    public string form2;

    public Form(string _name, int _shoe, string _email, string _cleat, string _gender, string _form2)
    {
        name = _name;
        shoe = _shoe;
        email = _email;
        cleat = _cleat;
        gender = _gender;

        form2 = _form2;
    }

    public static string IntToShoe(int inp)
    {
        if(inp == 0)
        {
            return "Predator";
        }
        else if(inp == 1)
        {
            return "Nemeziz";
        }
        else if (inp == 2)
        {
            return "X";
        }
        else
        {
            return "ERROR";
        }
    }
}
