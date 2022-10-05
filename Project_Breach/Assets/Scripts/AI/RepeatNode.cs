using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    protected override void OnStart() {

    }

    protected override void OnStop() {

    }

    protected override State OnUpdate() {
        //the behvaior of the can be customized to loop when successful, when failing, etc.
        child.Update();
        return State.Running; //this node will never return anything but Running (creating a loop in the tree)
    }
}
