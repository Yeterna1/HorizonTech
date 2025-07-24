
using UnityEngine;
using UnityEngine.AI;

public class PetState : MonoBehaviour{

    protected Pet Controller;

    protected virtual void Awake(){
        Controller = GetComponent<Pet>();
    }

    public virtual void Construct(){ }
    public virtual void Execute(){ }
    public virtual void Destruct(){ }

    public virtual void Transition(){ }
}

