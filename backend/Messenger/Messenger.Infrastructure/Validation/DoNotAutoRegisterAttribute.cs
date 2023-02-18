namespace Messenger.Infrastructure.Validation;

public class DoNotAutoRegisterAttribute : Attribute
{
    
    public static bool IsAutoRegistered(Type type)
    {
        return !type.GetCustomAttributes(typeof(DoNotAutoRegisterAttribute), false).Any();
    }
}
