namespace BudgetTracker.Core.Enums
{
    public enum ValidationError
    {
        LoginInactiveUserExists,
        LoginInUse,
        EmailInUse,
        PasswordsDoNotMatch,
        InvalidRegisterGuid,
        ActivateUserFailed,
        UserAlreadyActivated
    }
}
