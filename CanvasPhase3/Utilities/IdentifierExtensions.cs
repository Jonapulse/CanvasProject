
namespace CanvasPhase3.Utilities
{
    public static class IdentifierExtensions
    {
        public static string ToDisplayId(this int uid)
        {
            // e.g. 1042 -> "u0001042"
            return "u" + uid.ToString("D7");
        }
    }
}