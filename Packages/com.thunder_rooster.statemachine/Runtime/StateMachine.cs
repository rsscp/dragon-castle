// -----------------------------------------------------------------------------
//
// Use this runtime example C# file to develop runtime code.
//
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Thunderrooster.Statemachine
{
    /// <summary>
    /// Provide a general description of the public class.
    /// </summary>
    /// <remarks>
    /// Packages require XmlDoc documentation for ALL Package APIs.
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/xml-documentation-comments
    /// </remarks>
    /// 
    public class StateMachine
    {
        /// <summary>
        /// Provide a description of what this private method does.
        /// </summary>
        /// <param name="parameter1"> Description of parameter 1 </param>
        /// <param name="parameter2"> Description of parameter 2 </param>
        /// <param name="parameter3"> Description of parameter 3 </param>
        /// <returns> Description of what the function returns </returns>


        //private State[] states;
        //private int index;
        private List<Statee> registry;
        private Statee currentState;

        public void update()
        {
            currentState = currentState.getNextState();
            currentState.update();
        }

        public bool register(Statee[] newStates)
        {
            List<Statee> tempRegistry = new List<Statee>(registry);

            // Verify if any state is already registered, register otherwise
            foreach (Statee s in newStates)
                if (!tempRegistry.Contains(s))
                    tempRegistry.Add(s);

            // Verify if all transitions result in registered states
            foreach (Statee s in tempRegistry)
                foreach (Transitionn t in s.transitions)
                    if (!tempRegistry.Contains(t.resultState))
                        return false;

            registry = tempRegistry;
            
            return true;
        }

        public void force(string stateName) //TODO kinda fragile...
        {
            Statee nextState = getFromRegistry(stateName);
            if (nextState != null)
            {
                currentState.exit();
                currentState = nextState;
                currentState.enter();
            }
        }

        private Statee getFromRegistry(string name, List<Statee> registry)
        {
            foreach (Statee s in registry)
                if (s.name == name)
                    return s;
            return null;
        }

        private Statee getFromRegistry(string name)
        {
            return getFromRegistry(name, registry);
        }
    }
}