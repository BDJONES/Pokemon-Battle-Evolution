using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreenUIElements : MonoBehaviour
{
    public UIDocument uIDocument;
    public Button HostButton
    {
        get
        {
            if (uIDocument == null || uIDocument.rootVisualElement == null)
            {
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement networkButtons = screen.Query<VisualElement>("NetworkButtons");
                if (networkButtons != null)
                {
                    Button hostButton = networkButtons.Query<Button>("HostButton");
                    if (hostButton != null)
                    {
                        return hostButton;
                    }
                }
            }
            return null;
        }
    }
    public Button ClientButton { 
        get 
        {
            if (uIDocument == null || uIDocument.rootVisualElement == null)
            {
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement networkButtons = screen.Query<VisualElement>("NetworkButtons");
                if (networkButtons != null)
                {
                    Button clientButton = networkButtons.Query<Button>("ClientButton");
                    if (clientButton != null)
                    {
                        return clientButton;
                    }
                }
            }
            return null;
        } 
    }
    public Button JoinButton
    {
        get
        {
            if (uIDocument == null || uIDocument.rootVisualElement == null)
            {
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement networkButtons = screen.Query<VisualElement>("NetworkButtons");
                if (networkButtons != null)
                {
                    Button joinButton = networkButtons.Query<Button>("JoinButton");
                    if (joinButton != null)
                    {
                        return joinButton;
                    }
                }
            }
            return null;
        }
    }
}
