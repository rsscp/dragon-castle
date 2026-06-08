using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

public class Transition
{
    public Func<bool> condition;
    public State resultState;

    public Transition(Func<bool> condition, State resultState)
    {
        this.condition = condition;
        this.resultState = resultState;
    }
}

public class State
{
    public string name;
    public Action update;
    public Action enter;
    public Action exit;
    public Transition[] transitions;

    public State(Action update, Action enter, Action exit)
    {
        this.update = update;
        this.enter = enter;
        this.exit = exit;
        this.transitions = transitions;
    }

    public State getNextState()
    {
        foreach (Transition t in transitions)
            if (t.condition())
                return t.resultState;
        return this;
    }
} 