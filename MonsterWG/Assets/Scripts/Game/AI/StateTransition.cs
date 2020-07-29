using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game.AI
{
    [Serializable]
    public class StateTransition
    {
        [SerializeField] private State nextState = null;
        [SerializeField] private List<StateTransitionCondition> conditions = new List<StateTransitionCondition>();

        public State NextState => nextState;

        public bool ShouldTransition()
        {
            return conditions.AreMet();
        }
    }
}