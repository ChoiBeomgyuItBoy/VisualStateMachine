using System;
using RainbowAssets.Utils;
using UnityEngine;

public class InputReader : MonoBehaviour, IPredicateEvaluator
{
    public bool? Evaluate(string predicate, string[] parameters)
    {
        if(predicate == "Pressed Input")
        {
            bool parsed = Enum.TryParse(parameters[0], out KeyCode keyCode);

            if(parsed)
            {
                return Input.GetKey(keyCode);
            }
        }

        return false;
    }
}
