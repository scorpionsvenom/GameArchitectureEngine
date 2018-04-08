using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameArchitectureEngine
{
    public class FSM
    {
        private object owner;
        private List<State> states;

        private State currentState;

        public FSM() : this(null)
        {
        }


        public FSM(object owner)
        {
            this.owner = owner;
            states = new List<State>();
            currentState = null;
        }

        public void Initialise(string stateName)
        {
            currentState = states.Find(state => state.Name.Equals(stateName));

            if (currentState != null)
            {
                currentState.Enter(owner);
            }
        }

        public void AddState(State state)
        {
            states.Add(state);
        }

        public void Update(GameTime gameTime)
        {
            if (currentState == null) return;

            foreach (Transition t in currentState.Transitions)
            {
                if (t.Condition())
                {
                    currentState.Exit(owner);
                    currentState = t.NextState;
                    currentState.Enter(owner);
                    break;
                }
            }

            currentState.Execute(owner, gameTime);
        }
    }
}
