using RainbowAssets.Utils;
using UnityEngine;

public class Messenger : MonoBehaviour, IAction
{
    public void DoAction(string actionID, string[] parameters)
    {
        if(actionID == "Print Message")
        {
            foreach(var parameter in parameters)
            {
                print(parameter);
            }
        }
    }
}
