namespace Messenger.Core.Model.ConversationAggregate.Attachment;

public class GeolocationAttachment : IAttachment
{
    public static string Discriminator => "GeoPoint";
    
    /// <summary>
    /// Стоимость геолакации
    /// </summary>
    public double Cost => 0;
}
