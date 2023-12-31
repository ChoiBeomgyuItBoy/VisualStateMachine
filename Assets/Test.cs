using System;
using RainbowAssets.Utils;
using UnityEngine;

public class Test : MonoBehaviour, IAction, IPredicateEvaluator
{
    public void DoAction(string actionID, string[] parameters)
    {
        if(actionID == "Print Message")
        {
            foreach(var message in parameters)
            {
                Debug.Log(message);
            }
        }
    }

    public bool? Evaluate(string predicate, string[] parameters)
    {
        if(predicate == "Pressed Input")
        {
            if(Enum.TryParse(parameters[0], out KeyCode keyCode))
            {
                return Input.GetKey(keyCode);
            }
        }

        return false;
    }
}
