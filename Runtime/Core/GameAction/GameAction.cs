using Script.Runtime.Core.Data;
using UnityEngine;

namespace Script.Runtime.Core.GameAction
{
    [CreateAssetMenu(fileName = "GameAction", menuName = "Core/Game Action", order = 0)]
    public class GameAction : ScriptableObject, IGameData
    {
        public virtual void Execute(GameActionContext context)
        {
            Debug.Log($"Executing {name} from {context.Caller} on {context.Receiver}.");
        }
    }
}