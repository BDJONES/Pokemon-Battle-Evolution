using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ViewBuilder : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset InBattlePartyWidget;
    private Menus previousMenu;
    
    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;
    }

    private void OnDisable()
    {
        UIController.OnMenuChange -= HandleMenuChange;
    }

    private void HandleMenuChange(Menus menu)
    {
        
        if (menu == Menus.InBattlePartyMenu)
        {
            PopulateUI();
        }
        else
        {
            if (previousMenu == Menus.InBattlePartyMenu)
            {
                var widgetHolder = GameObject.Find("WidgetHolder");
                var pokemon1Controller = widgetHolder.GetComponent<Pokemon1Controller>();
                var pokemon2Controller = widgetHolder.GetComponent<Pokemon2Controller>();
                var pokemon3Controller = widgetHolder.GetComponent<Pokemon3Controller>();
                var pokemon4Controller = widgetHolder.GetComponent<Pokemon4Controller>();
                var pokemon5Controller = widgetHolder.GetComponent<Pokemon5Controller>();
                var pokemon6Controller = widgetHolder.GetComponent<Pokemon6Controller>();
                Destroy(pokemon1Controller);
                Destroy(pokemon2Controller);
                Destroy(pokemon3Controller);
                Destroy(pokemon4Controller);
                Destroy(pokemon5Controller);
                Destroy(pokemon6Controller);
                Destroy(GameObject.Find("WidgetHolder"));
                //RemoveUI();
            }
        }
        previousMenu = menu;
    }

    private void PopulateUI()
    {
        int index = 1;
        GameObject WidgetHolder = new GameObject
        {
            name = "WidgetHolder"
        };
        foreach (Pokemon pokemon in GameManager.Instance.trainer1.pokemonTeam)
        {
            TemplateContainer newWidget = InBattlePartyWidget.Instantiate();
            newWidget.name = $"Pokemon{index}";
            if (pokemon != null)
            {
                Label nameText = newWidget.Q<Label>("Name");
                Label levelText = newWidget.Q<Label>("Level");
                Label genderText = newWidget.Q<Label>("Gender");
                ProgressBar progressBar = newWidget.Q<ProgressBar>("HPBar");
                Label hpStatText = progressBar.Q<Label>("HPStat");
                nameText.text = pokemon.GetSpeciesName();
                levelText.text = $"Lv. {pokemon.GetLevel()}";
                progressBar.highValue = pokemon.GetMaxHPStat();
                progressBar.value = pokemon.GetHPStat();
                hpStatText.text = $"{pokemon.GetHPStat()}/{pokemon.GetMaxHPStat()}";


            }
            if (index <= 3)
            {
                UIController.Instance.currentUI.rootVisualElement.Q("Top_Row").Add(newWidget);                
                if (index == 1 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon1Controller>();
                }
                else if (index == 2 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon2Controller>();
                }
                else if (index == 3 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon3Controller>();
                }
            }
            else
            {
                UIController.Instance.currentUI.rootVisualElement.Q("Bottom_Row").Add(newWidget);
                if (index == 4 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon4Controller>();
                }
                else if (index == 5 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon5Controller>();
                }
                else if (index == 6 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon6Controller>();
                }
            }
            index++;
        }
        //RemoveUI();
    }

    private void RemoveUI()
    {
        for (int i = 1; i <= 6; i++)
        {
            var element = UIController.Instance.currentUI.rootVisualElement.Q<VisualElement>($"Pokemon{i}");
            if (element != null)
            {
                Debug.Log("Removing duplicates");
                if (i <= 3)
                {
                    UIController.Instance.currentUI.rootVisualElement.Q("Top_Row").Remove(element);
                }
                else
                {
                    UIController.Instance.currentUI.rootVisualElement.Q("Bottom_Row").Remove(element);
                }
            }
        }
    }
}
