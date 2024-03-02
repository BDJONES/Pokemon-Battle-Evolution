using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackInfoUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uIDocument;
    
    public VisualElement AttackInfo
    {
        get
        {
            if (uIDocument == null || uIDocument.rootVisualElement == null)
            {
                return null;
            }

            VisualElement content = uIDocument.rootVisualElement.Query<VisualElement>("Content");
            if (content != null)
            {
                TemplateContainer attackInfoWidget = content.Query<TemplateContainer>("AttackInfoWidget");
                if (attackInfoWidget != null)
                {
                    VisualElement widgetContent = attackInfoWidget.Query<VisualElement>("Content");
                    if (widgetContent != null)
                    {
                        Button attackInfo = widgetContent.Query<Button>("AttackInfo");
                        if (attackInfo != null)
                        {
                            return attackInfo;
                        }
                    }
                }
            }

            return null;
        }
    }

    public Button BackButton { 
        get
        {
            if (uIDocument == null || uIDocument.rootVisualElement == null)
            {
                return null;
            }

            VisualElement content = uIDocument.rootVisualElement.Query<VisualElement>("Content");
            if (content != null)
            {
                Debug.Log("Found the Content");
                TemplateContainer attackInfoWidget = content.Query<TemplateContainer>("AttackInfoWidget");
                if (attackInfoWidget != null)
                {
                    Debug.Log("Found the Widget");
                    VisualElement widgetContent = attackInfoWidget.Query<VisualElement>("Content");
                    if (widgetContent != null)
                    {
                        Debug.Log("Found the widgets content");
                        Button backButton = widgetContent.Query<Button>("BackButton");
                        if (backButton != null)
                        {
                            Debug.Log("Found the Back Button");
                            return backButton;
                        }
                    }
                }
            }
            Debug.Log("Unable to find the item");
            return null;
        } 
    }
}
