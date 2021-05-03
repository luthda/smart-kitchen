using System;

namespace Hsr.CloudSolutions.SmartKitchen.Util
{
    public static class ExceptionExtensions
    {

        public static string CreateExceptionDialogMessage(this Exception exception)
        {
            var message = $"An unexpected exception occured!\r\n{ExceptionMessage(exception)}";
            var innerstException = exception.GetInnerstException();
            if (innerstException != null)
            {
                message += $"\r\n\r\nInnerst exception:\r\n{ExceptionMessage(innerstException)}";
            }
            return message;

            // Local function
            string ExceptionMessage(Exception ex) => $"Type: {ex.GetType().Name}\r\nMessage: {ex.Message}";
        }

        public static Exception GetInnerstException(this Exception exception)
        {
            if (exception.InnerException == null)
            {
                return null;
            }
            var ex = exception;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
    }
}
