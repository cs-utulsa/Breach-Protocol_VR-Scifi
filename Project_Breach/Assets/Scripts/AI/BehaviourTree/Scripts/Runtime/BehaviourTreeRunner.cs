using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace behaviorNameSpace {
    public class BehaviourTreeRunner : MonoBehaviour {

        // The main behaviour tree asset
        public BehaviourTree tree;
        public PhotonView photonView;

        // Storage container object to hold game object subsystems
        Context context;

        // Start is called before the first frame update
        void Start() {
            context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.Bind(context);
            photonView = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update() {
            if (tree && photonView.IsMine) {
                tree.Update();
            }
        }

        Context CreateBehaviourTreeContext() {
            return Context.CreateFromGameObject(gameObject);
        }

        private void OnDrawGizmosSelected() {
            if (!tree) {
                return;
            }

            BehaviourTree.Traverse(tree.rootNode, (n) => {
                if (n.drawGizmos) {
                    n.OnDrawGizmos();
                }
            });
        }
    }
}