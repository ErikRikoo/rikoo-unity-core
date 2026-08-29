namespace Rikoo.Framework.Core.Utilities
{
    public interface IFilterable
    {
        bool IsMatch(string searchString);
    }
}