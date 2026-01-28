using Sirenix.OdinInspector;
using UnityEngine;

namespace Rikoo.Core.GameAction
{
    [CreateAssetMenu(fileName = "GameActionList", menuName = "Core/Game Action List", order = 0)]
    public class GameActionList : GameAction
    {
        [SerializeField, InlineEditor] private GameAction[] actions;

        public override void Execute(GameActionContext context)
        {
            for (var i = 0; i < actions.Length; i++)
            {
                GameAction action = actions[i];
                if (action == null)
                {
                    Debug.LogError($"Game Action List({name}): Action at index {i} is null.");
                    continue;
                }
                action.Execute(context);
            }
        }
    }
}