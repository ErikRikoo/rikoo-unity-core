using UnityEngine;

namespace Core.GameplayElementCode.Lifecycle
{
    public class GameplayElementUpdateHandler : MonoBehaviour
    {
        public GameplayElement root;

        public void OnFetched() {}
        
        private void Update()
        {
            root.UpdateBehaviors();
        }
        
        private void FixedUpdate()
        {
            root.FixedUpdateBehaviors();
        }
    }
}