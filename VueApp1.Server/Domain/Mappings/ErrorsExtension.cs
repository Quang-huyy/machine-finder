using VueApp1.Server.Domain.Enums;
namespace VueApp1.Server.Domain.Mappings
{
    public static class ErrorsExtension
    {
        public static string toString(this Errors error)
        {
            return error switch
            {
                Errors.MACHINE_NOT_FOUND => "Machine not found",
                Errors.INVALID_TAG => "Invalid tag",
                Errors.EMPTY_FIELD => "Empty field",
                Errors.SQL_UPDATE_FAILED => "SQL update failed",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
