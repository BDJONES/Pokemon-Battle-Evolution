using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIEventSubscriptionManager : Singleton<UIEventSubscriptionManager>
{
    private static List<Button> trainer1Buttons = new List<Button>();
    private static List<Action> trainer1Functions = new List<Action>();
    private static List<Button> trainer2Buttons = new List<Button>();
    private static List<Action> trainer2Functions = new List<Action>();

    public static void Subscribe(Button button, Action func, int type) // type = 1 is for Host, type = 2 is for Client
    {
        Debug.Log("Added a new subscription");
        //TemplateContainer templateContainer = new TemplateContainer();
        if (type == 1)
        {
            button.clicked += func;
            trainer1Buttons.Add(button);
            trainer1Functions.Add(func);
        }
        else if (type == 2)
        {
            button.clicked += func;
            trainer2Buttons.Add(button);
            trainer2Functions.Add(func);
        }
    }
    
    public static void UnsubscribeAll(int type)
    {
        Debug.Log("Unsubscribed from everything");
        if (type == 1)
        {
            while (trainer1Buttons.Count > 0 && trainer1Functions.Count > 0)
            {
                Button button = trainer1Buttons[trainer1Buttons.Count - 1];
                Action function = trainer1Functions[trainer1Functions.Count - 1];
                button.clicked -= function;
                trainer1Buttons.RemoveAt(trainer1Buttons.Count - 1);
                trainer1Functions.RemoveAt(trainer1Functions.Count - 1);
            }
        }
        else if (type == 2)
        {
            while (trainer2Buttons.Count > 0 && trainer2Functions.Count > 0)
            {
                Button button = trainer2Buttons[trainer2Buttons.Count - 1];
                Action function = trainer2Functions[trainer2Functions.Count - 1];
                button.clicked -= function;
                trainer2Buttons.RemoveAt(trainer2Buttons.Count - 1);
                trainer2Functions.RemoveAt(trainer2Functions.Count - 1);
            }
        }
    }
}
