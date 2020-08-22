using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hacker : MonoBehaviour
{
    string menuHint = "Type menu to return to the menu";

    // Game Configuration Data
    string[] level1Passwords = { "books", "aisle", "self", "password", "font", "borrow" };
    string[] level2Passwords = { "prisoner", "handcuffs", "holster", "uniform", "arrest" };
    string[] level3Passwords = { "python", "ruby", "perl", "java", "c#" };

    // Game States
    int level;
    enum Screen { MainMenu, Password, Win};
    Screen currentScreen = Screen.MainMenu;
    string password;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    void ShowMainMenu()
    {
        currentScreen = Screen.MainMenu;
        string message = "Enter one of the following options" +
            "\n1- Hack public library." +
            "\n2- Hack police station." +
            "\n3- Hack NASA.";
        Terminal.ClearScreen();
        Terminal.WriteLine(message);
    }

    void OnUserInput(string input)
    {
        if (input == "menu")
        {
            ShowMainMenu();
        }
        else if (input == "quit" || input == "exit" || input == "close")
        {
            Application.Quit();
        }
        else if (currentScreen == Screen.MainMenu)
        {
            RunMainMenu(input);
        }
        else if (currentScreen == Screen.Password)
        {
            CheckPassword(input);
        }
    }

    void RunMainMenu(string input)
    {
        bool isValidLevelNumber = (input == "1" || input == "2" || input == "3");
        if (isValidLevelNumber)
        {
            level = int.Parse(input);
            StartGame();
        }
        else
        {
            Terminal.WriteLine("Invalid option");
            Terminal.WriteLine(menuHint);
        }
    }

    void StartGame()
    {
        currentScreen = Screen.Password;
        Terminal.ClearScreen();
        SetRandomPassword();
        Terminal.WriteLine("Enter your password, hint: " + password.Anagram());
        Terminal.WriteLine(menuHint);
    }

    void SetRandomPassword()
    {
        switch (level)
        {
            case 1:
                int indexLevel1 = Random.Range(0, level1Passwords.Length);
                password = level1Passwords[indexLevel1];
                break;
            case 2:
                int indexLevel2 = Random.Range(0, level2Passwords.Length);
                password = level2Passwords[indexLevel2];
                break;
            case 3:
                int indexLevel3 = Random.Range(0, level3Passwords.Length);
                password = level3Passwords[indexLevel3];
                break;
            default:
                Debug.LogError("Invalid Option");
                break;
        }
    }

    void CheckPassword(string input)
    {
        if (input == password)
        {
            DisplayWinScreen();
        }
        else
        {
            StartGame();
        }
    }

    void DisplayWinScreen()
    {
        currentScreen = Screen.Win;
        Terminal.ClearScreen();
        ShowLevelReward();
        Terminal.WriteLine(menuHint);
    }

    void ShowLevelReward()
    {
        switch (level)
        {
            case 1:
                Terminal.WriteLine("Have a PC...");
                Terminal.WriteLine(@"
.--.
|__| .-------.
|=.| |.-----.|
|--| || KCK ||
|  | |'-----'|
|__|~')_____('
");
                break;
            case 2:
                Terminal.WriteLine("Have a PC...");
                Terminal.WriteLine(@"
.--.
|__| .-------.
|=.| |.-----.|
|--| || KCK ||
|  | |'-----'|
|__|~')_____('
");
                break;
            case 3:
                Terminal.WriteLine("Have a PC...");
                Terminal.WriteLine(@"
.--.
|__| .-------.
|=.| |.-----.|
|--| || KCK ||
|  | |'-----'|
|__|~')_____('
");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
