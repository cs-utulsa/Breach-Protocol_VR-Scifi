using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace behaviorNameSpace {
    public class Wait : ActionNode {
        public float duration = 1;
        float startTime;

        protected override void OnStart() {
            startTime = Time.time;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (context.aiAgent.aiHealth.GetIsDead())
            {
                return State.Failure;
            }

            if (Time.time - startTime > duration) {
                return State.Success;
            }
            return State.Running;
        }
    }
}
