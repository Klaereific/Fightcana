using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{

    protected Dictionary<EState,BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    public BaseState<EState> currentState{get; protected set;}

    protected bool isTransitioningState = false;

   void Start(){
    currentState.EnterState();
   }

   void FixedUpdate(){
    EState nextStateKey = currentState.GetNextState();
    if(!isTransitioningState && nextStateKey.Equals(currentState.StateKey)){
        currentState.UpdateState();
    } else if (!isTransitioningState) {
        TransitionToState(nextStateKey);
    }
   }

   public void TransitionToState(EState stateKey)
   {
    isTransitioningState = true;
    currentState.ExitState();
    currentState = States[stateKey];
    currentState.EnterState();
    isTransitioningState = false;    
   }

   void OnTriggerEnter(Collider other){
    currentState.OnTriggerEnter(other);
   }

   void OnTriggerStay(Collider other){
    currentState.OnTriggerStay(other);
   }

   void OnTriggerExit(Collider other){
    currentState.OnTriggerExit(other);
   }
}
