﻿using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class base_state_machine
    {
        [Test]
        public void initial_set_state_switches_to_state()
        {
            var stateMachine = new StateMachine();
            IState firstState = Substitute.For<IState>();
            stateMachine.SetState(firstState);

            Assert.AreSame(firstState, stateMachine.CurrentState);
        }

        [Test]
        public void subsequent_set_state_switches_to_state()
        {
            var stateMachine = new StateMachine();
            IState firstState = Substitute.For<IState>();
            IState secondState = Substitute.For<IState>();

            stateMachine.SetState(firstState);
            Assert.AreSame(firstState, stateMachine.CurrentState);

            stateMachine.SetState(secondState);
            Assert.AreSame(secondState, stateMachine.CurrentState);
        }

        [Test]
        public void transition_switiches_state_when_condition_is_met()
        {
            var stateMachine = new StateMachine();
            IState firstState = Substitute.For<IState>();
            IState secondState = Substitute.For<IState>();

            // local method/function
            bool ShouldTransitionState() => true;
            stateMachine.AddTransition(firstState, secondState, ShouldTransitionState);

            stateMachine.SetState(firstState);
            Assert.AreSame(firstState, stateMachine.CurrentState);

            stateMachine.Tick();
            Assert.AreSame(secondState, stateMachine.CurrentState);
        }

        [Test]
        public void transition_does_not_switich_state_when_condition_is__net_met()
        {
            var stateMachine = new StateMachine();
            IState firstState = Substitute.For<IState>();
            IState secondState = Substitute.For<IState>();

            // local method/function
            bool ShouldTransitionState() => false;
            stateMachine.AddTransition(firstState, secondState, ShouldTransitionState);

            stateMachine.SetState(firstState);
            Assert.AreSame(firstState, stateMachine.CurrentState);

            stateMachine.Tick();
            Assert.AreSame(firstState, stateMachine.CurrentState);
        }

        [Test]
        public void transition_does_not_switich_state_when_not_in_correct_source_state()
        {
            var stateMachine = new StateMachine();
            IState firstState = Substitute.For<IState>();
            IState secondState = Substitute.For<IState>();
            IState thirdState = Substitute.For<IState>();

            // local method/function
            bool ShouldTransitionState() => true;
            stateMachine.AddTransition(secondState, thirdState, ShouldTransitionState);

            stateMachine.SetState(firstState);
            Assert.AreSame(firstState, stateMachine.CurrentState);

            stateMachine.Tick();
            Assert.AreSame(firstState, stateMachine.CurrentState);
        }
        
        [Test]
        public void transition_from_any_switiches_state_when_condition_is_met()
        {
            var stateMachine = new StateMachine();
            IState firstState = Substitute.For<IState>();
            IState secondState = Substitute.For<IState>();

            // local method/function
            bool ShouldTransitionState() => true;
            stateMachine.AddAnyTransition(secondState, ShouldTransitionState);

            stateMachine.SetState(firstState);
            Assert.AreSame(firstState, stateMachine.CurrentState);

            stateMachine.Tick();
            Assert.AreSame(secondState, stateMachine.CurrentState);
        }
    }
}