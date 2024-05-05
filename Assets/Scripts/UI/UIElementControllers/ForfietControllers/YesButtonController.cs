using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        NetworkCommands.UIControllerCreated += () =>
        {

        };
    }
}
