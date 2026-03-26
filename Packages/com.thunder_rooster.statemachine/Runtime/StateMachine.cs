// -----------------------------------------------------------------------------
//
// Use this runtime example C# file to develop runtime code.
//
// -----------------------------------------------------------------------------

using System;
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

        private State[] states;
        private int index;

        public StateMachine(int initState, State[] states)
        {
            index = initState;
            this.states = states;
        }

        public void update()
        {
            int newIndex = transition();
                
            if (newIndex != index)
            {
                states[index].exit();
                states[newIndex].enter();
            }

            index = newIndex;
            states[index].update();
        }

        public void force(int state)
        {
            this.index = state;
        }

        private int transition()
        {
            int newIndex = index;

            foreach (Transition transition in states[index].transitions)
            {
                if (transition.condition())
                {
                    newIndex = transition.resultState;
                    break;
                }
            }

            return newIndex;
        }
    }
}