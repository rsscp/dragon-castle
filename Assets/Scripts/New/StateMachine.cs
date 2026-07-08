using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using NUnit.Framework.Interfaces;
using UnityEngine;

public abstract class StateBehaviour
{
    public abstract void Enter();
    public abstract void Stay();
    public abstract void Exit();
}

public class StateMachine : MonoBehaviour
{
    private const string NO_STATES_MSG = "A valid state machine must have at least one state. No states have been added.";
    private const string NO_CURR_STATE_MSG = "An initial state has not been definied for this state machine";
    private string STATE_NOT_FOUND(string name)
    {
        return $"The state '{name}' was not found";
    }
    private string NO_DIRECT_TRANSITION(string from, string to)
    {
        return $"The state '{from}' has no allowed direct transition to state '{to}'";
    }

    private class State
    {
        public StateBehaviour Behaviour { get; set; }
        public List<Transition> ConditionalTransitions { get; set; }
        public Dictionary<string, Transition> DirectTransitions { get; set; }

        public State(StateBehaviour behaviour)
        {
            Behaviour = behaviour;
            ConditionalTransitions = new List<Transition>();
            DirectTransitions = new Dictionary<string, Transition>();
        }

        public bool HasDirectTo(string name)
        {
            return DirectTransitions.TryGetValue(name, out var result);
        }
    }
    private class Transition
    {
        public string ResultName { get; set; }
        public State ResultState { get; set; }
        public Func<bool> Check { get; set; }

        public Transition(string resultName, State resultState, Func<bool> check)
        {
            ResultName = resultName;
            ResultState = resultState;
            Check = check;
        }

        public Transition(string resultName, State resultState)
        {
            ResultName = resultName;
            ResultState = resultState;
            Check = () => false;
        }
    }

    private Dictionary<string, State> _states = new Dictionary<string, State>();
    private string _current = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _check();
    }

    // Update is called once per frame
    void Update()
    {
        _transition();
        _currentState().Behaviour.Stay();
    }

    private void _check()
    {
        if (_current == null)
            throw new Exception(NO_STATES_MSG); //TODO Use more specific exception
        if (_states.Count == 0)
            throw new Exception(NO_CURR_STATE_MSG); //TODO Use more specific exception
    }

    private State _currentState()
    {
        return _states[_current];
    }

    private void _transitionTo(string name)
    {
        _currentState().Behaviour.Exit();
        _current = name;
        _currentState().Behaviour.Enter();
    }

    private void _transition()
    {
        foreach (Transition transition in _currentState().ConditionalTransitions)
            if (transition.Check())
                _transitionTo(transition.ResultName);
    }

    public void TransitionTo(string name)
    {
        if (_currentState().HasDirectTo(name))
            _transitionTo(name);
        else
            throw new Exception(NO_DIRECT_TRANSITION(_current, name));
    }

    public void AddState(string name, StateBehaviour behaviour)
    {
        State newState = new State(behaviour);
        _states.Add(name, newState);
    }

    public void AddTransition(string from, string to, Func<bool> condition)
    {

        State fromState = _states[from];
        State toState = _states[to];
        Transition transition = new Transition(to, toState, condition);

        fromState.ConditionalTransitions.Add(transition);
    }

    public void AddTransition(string from, string to)
    {

        State fromState = _states[from];
        State toState = _states[to];
        Transition transition = new Transition(to, toState);

        fromState.DirectTransitions.Add(to, transition);
    }

    public void SelectStartingState(string name)
    {
        if (!_states.ContainsKey(name))
            throw new Exception(STATE_NOT_FOUND(name));
        _current = name;
    }
}
