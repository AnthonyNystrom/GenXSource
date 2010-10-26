using System;

namespace NuGenSVisualLib.Exceptions
{
    public class UserLevelException : ApplicationException
    {
        public enum ExceptionType
        {
            FileLoading,
            GraphicsDevice,
            Unknown
        }

        readonly ExceptionType exceptionType;
        readonly Type location;

        public UserLevelException(string message, ExceptionType type, Type location,
                                  Exception innerException)
            : base(message, innerException)
        {
            this.exceptionType = type;
            this.location = location;
        }

        #region Properties

        public ExceptionType TypeOfException
        {
            get { return exceptionType; }
        }

        public Type Location
        {
            get { return location; }
        }
        #endregion
    }
}
