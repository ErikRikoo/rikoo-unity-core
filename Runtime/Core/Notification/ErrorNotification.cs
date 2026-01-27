namespace Script.Runtime.Core.Notification
{
    public static partial class ErrorType
    {
        
    }
    
    public struct ErrorNotification: IGameNotification
    {
        public static readonly string ClassName = nameof(ErrorNotification);
        public string Name => ClassName;
        
        public object ErrorData;

        public ErrorNotification(object errorData)
        {
            ErrorData = errorData;
        }

        public static string MakeLabel(string type, string reason = "*")
        {
            return $"{nameof(ErrorNotification)}({type}-{reason})";
        }
    }
}