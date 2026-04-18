using System.Collections.Generic;
using UnityEngine;
using TappyTale.PostNavCoverFSM.Runtime.AI;

namespace TappyTale.PostNavCoverFSM.Runtime.FSM
{
    [DisallowMultipleComponent]
    public class StateMachineController : MonoBehaviour
    {
        [SerializeField] private FSMState initialState;
        [SerializeField] private List<FSMStateDefinition> states = new List<FSMStateDefinition>();

        private readonly Dictionary<FSMState, List<FSMTransitionDefinition>> transitionMap = new Dictionary<FSMState, List<FSMTransitionDefinition>>();
        private AIController owner;
        private FSMState currentState;

        public FSMState CurrentState => currentState;
        public IReadOnlyList<FSMStateDefinition> States => states;

        public void Initialize(AIController controller)
        {
            owner = controller;
            RebuildLookup();
            ChangeState(initialState);
        }

        public void Tick(float deltaTime)
        {
            if (owner == null || currentState == null)
            {
                return;
            }

            if (transitionMap.TryGetValue(currentState, out var transitions))
            {
                for (int i = 0; i < transitions.Count; i++)
                {
                    FSMTransitionDefinition transition = transitions[i];
                    if (transition?.Condition != null && transition.TargetState != null && transition.Condition.Evaluate(owner))
                    {
                        ChangeState(transition.TargetState);
                        return;
                    }
                }
            }

            currentState.OnTick(owner, deltaTime);
        }

        public void SetAuthoringData(FSMState newInitialState, List<FSMStateDefinition> newStates)
        {
            initialState = newInitialState;
            states = newStates ?? new List<FSMStateDefinition>();
            RebuildLookup();
        }

        public void ChangeState(FSMState nextState)
        {
            if (nextState == currentState)
            {
                return;
            }

            currentState?.OnExit(owner);
            currentState = nextState;
            owner?.Blackboard?.GetType();
            currentState?.OnEnter(owner);
        }

        private void RebuildLookup()
        {
            transitionMap.Clear();
            for (int i = 0; i < states.Count; i++)
            {
                FSMStateDefinition definition = states[i];
                if (definition?.State == null)
                {
                    continue;
                }

                transitionMap[definition.State] = definition.Transitions ?? new List<FSMTransitionDefinition>();
            }
        }
    }
}
