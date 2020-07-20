namespace GitGetter.Interfaces
{
    /// <summary>
    /// Interface for Error Reporter functionality. It allows a method that performs a task to give back error messages to the caller, without exceptions being thrown.
    /// </summary>
    public interface IErrorReporter
    {
        /// <summary>
        /// Clear the error text stored in the IErrorReporter instance.
        /// </summary>
        void ClearError();

        /// <summary>
        /// Returns true if the IErrorReporter instance contains error text; false otherwise.
        /// </summary>
        /// <returns></returns>
        bool HasError();

        /// <summary>
        /// Append msg to the IErrorReporter instance. If there is existing error text, it will be preserved and a newline is inserted between the existing text and the new msg.
        /// </summary>
        /// <param name="msg"></param>
        void ShowError(string msg);
    }
}