
namespace Rikoo.Core.GlobalHandlerManagement
{
    public class GlobalHandler<T> where T : GlobalHandler<T>
    {
        private static T instance;
        
        public static T Instance => instance;

        public GlobalHandler()
        {
            instance = this as T;
            // Register into global handler debugger
        }
    }
}