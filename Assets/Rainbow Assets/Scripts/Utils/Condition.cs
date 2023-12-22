using System.Collections.Generic;
using UnityEngine;

namespace RainbowAssets.Utils
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach(var disjunction in and)
            {
                if(!disjunction.Check(evaluators))
                {
                    return false;
                }
            }

            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField] Predicate[] or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach(var predicate in or)
                {
                    if(predicate.Check(evaluators))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] string predicate;
            [SerializeField] string[] parameters;
            [SerializeField] bool negate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach(var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);

                    if(result == null)
                    {
                        return false;
                    }

                    if(result == negate)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
