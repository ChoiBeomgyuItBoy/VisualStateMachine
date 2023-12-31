using System.Linq;

namespace RainbowAssets.StateMachine
{
    public class EntryState : State 
    { 
        public string GetEntryStateID()
        {
            return GetTransitions().ToList()[0].GetTrueStateID();
        }
    }
}