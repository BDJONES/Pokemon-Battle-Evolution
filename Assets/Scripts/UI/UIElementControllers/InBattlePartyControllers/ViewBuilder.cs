using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ViewBuilder : NetworkBehaviour
{
    [SerializeField] private VisualTreeAsset InBattlePartyWidget;
    [SerializeField] TrainerController trainerController;
    private Menus previousMenu;
    private UIController uIController;
    public static event Action OnViewCreated;

    private void OnEnable()
    {
        //if (IsOwner)
        //{   

            NetworkCommands.UIControllerCreated += () =>
            {
                uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
                trainerController = transform.parent.gameObject.transform.parent.gameObject.GetComponent<TrainerController>();
                uIController.OnHostMenuChange += HandleMenuChange;
                uIController.OnClientMenuChange += HandleMenuChange;

            };
        //}
    }
    private void OnDisable()
    {
        if (uIController != null)
        {
            uIController.OnHostMenuChange -= HandleMenuChange;
            uIController.OnClientMenuChange -= HandleMenuChange;
        }
    }

    private void HandleMenuChange(Menus menu)
    {
        if (IsOwner)
        {
            if (menu == Menus.InBattlePartyMenu || menu == Menus.InBattlePartyDialogueScreen || menu == Menus.PokemonFaintedScreen)
            {
                Debug.Log("Creating the WidgetHolder");
                RemoveUI();
                PopulateUI();
            }
            else if (previousMenu == Menus.InBattlePartyMenu || previousMenu == Menus.InBattlePartyDialogueScreen || previousMenu == Menus.PokemonFaintedScreen)
            {
                Debug.Log("Removing the WidgetHolder");
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
                Destroy(widgetHolder);
            }
            previousMenu = menu;
        }
    }

    private void PopulateUI()
    {
        Debug.Log("Populating UI");
        int index = 1;
        GameObject WidgetHolder = new GameObject
        {
            name = "WidgetHolder"
        };
        foreach (Pokemon pokemon in trainerController.GetPlayer().GetPokemonTeam())
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
                uIController.currentUI.rootVisualElement.Q("Top_Row").Add(newWidget);                
                if (index == 1 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon1Controller>();
                    WidgetHolder.GetComponent<Pokemon1Controller>().SetTrainerController(trainerController);
                }
                else if (index == 2 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon2Controller>();
                    WidgetHolder.GetComponent<Pokemon2Controller>().SetTrainerController(trainerController);
                }
                else if (index == 3 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon3Controller>();
                    WidgetHolder.GetComponent<Pokemon3Controller>().SetTrainerController(trainerController);
                }
            }
            else
            {
                uIController.currentUI.rootVisualElement.Q("Bottom_Row").Add(newWidget);
                if (index == 4 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon4Controller>();
                    WidgetHolder.GetComponent<Pokemon4Controller>().SetTrainerController(trainerController);
                }
                else if (index == 5 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon5Controller>();
                    WidgetHolder.GetComponent<Pokemon5Controller>().SetTrainerController(trainerController);
                }
                else if (index == 6 && pokemon != null)
                {
                    WidgetHolder.AddComponent<Pokemon6Controller>();
                    WidgetHolder.GetComponent<Pokemon6Controller>().SetTrainerController(trainerController);
                }
            }
            index++;
        }
        OnViewCreated?.Invoke();
        //RemoveUI();
    }

    private void RemoveUI()
    {
        if (previousMenu == Menus.InBattlePartyMenu || previousMenu == Menus.InBattlePartyDialogueScreen || previousMenu == Menus.PokemonFaintedScreen)
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
            Destroy(widgetHolder);
            for (int i = 1; i <= 6; i++)
            {
                var element = uIController.currentUI.rootVisualElement.Q<VisualElement>($"Pokemon{i}");
                if (element != null)
                {
                    Debug.Log("Removing duplicates");
                    if (i <= 3)
                    {
                        uIController.currentUI.rootVisualElement.Q("Top_Row").Remove(element);
                    }
                    else
                    {
                        uIController.currentUI.rootVisualElement.Q("Bottom_Row").Remove(element);
                    }
                }
            }
        }
    }
}
