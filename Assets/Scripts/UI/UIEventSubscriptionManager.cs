using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIEventSubscriptionManager : Singleton<UIEventSubscriptionManager>
{
    private static List<Button> buttons = new List<Button>();
    private static List<Action> functions = new List<Action>();
    
    public static void Subscribe(Button button, Action func)
    {
        button.clicked += func;
        buttons.Add(button);
        functions.Add(func);
    }
    
    public static void UnsubscribeAll()
    {
        while (buttons.Count > 0 && functions.Count > 0)
        {
            Button button = buttons[buttons.Count - 1];
            Action function = functions[functions.Count - 1];
            button.clicked -= function;
            buttons.RemoveAt(buttons.Count - 1);
            functions.RemoveAt(functions.Count - 1);
        }
    }
}
