using System;
using System.Collections.Generic;

public class Transition
{
    public Func<bool> condition;
    public int resultState;

    public Transition(Func<bool> condition, int resultState)
    {
        this.condition = condition;
        this.resultState = resultState;
    }
}

public class State
{
    public Action update;
    public Action enter;
    public Action exit;
    public Transition[] transitions;

    public State(Action update, Action enter, Action exit, Transition[] transitions)
    {
        this.update = update;
        this.enter = enter;
        this.exit = exit;
        this.transitions = transitions;
    }
} 