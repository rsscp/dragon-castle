
using System;
using Unity.VisualScripting.Dependencies.NCalc;
/*
public class Transition
{
    public State result;
    public Func<bool> check;
}

public abstract class State
{
    public Transition[] transitions;

    public abstract void enter();
    public abstract void update();
    public abstract void exit();

    public State next()
    {
        State result = this;

        foreach (Transition t in transitions)
            if (t.check())
                result = t.result;

        return result;
    }

    public bool isValid()
    {
        return transitions.Length > 0;
    }
}

*/