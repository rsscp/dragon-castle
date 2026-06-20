using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

public class Transitionn
{
    public Func<bool> condition;
    public Statee resultState;

    public Transitionn(Func<bool> condition, Statee resultState)
    {
        this.condition = condition;
        this.resultState = resultState;
    }
}

public class Statee
{
    public string name;
    public Action update;
    public Action enter;
    public Action exit;
    public Transitionn[] transitions;

    public Statee(Action update, Action enter, Action exit)
    {
        this.update = update;
        this.enter = enter;
        this.exit = exit;
        this.transitions = transitions;
    }

    public Statee getNextState()
    {
        foreach (Transitionn t in transitions)
            if (t.condition())
                return t.resultState;
        return this;
    }
} 