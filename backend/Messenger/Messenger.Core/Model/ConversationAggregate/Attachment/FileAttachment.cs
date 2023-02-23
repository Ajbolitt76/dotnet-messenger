namespace Messenger.Core.Model.ConversationAggregate.Attachment;

public class FileAttachment : IAttachment, IHaveDiscriminator
{
    public static string Discriminator => "File";
    
    public required Guid FileId { get; set; }
    
    /// <summary>
    /// Имя файла при загрузке
    /// </summary>
    public required string UploadName { get; init; }
    
    /// <summary>
    /// Размер загрузки в байтах
    /// </summary>
    public required long UploadSize { get; init; }

    /// <summary>
    /// Стоимость файла 1КБ - 1 очко
    /// </summary>
    public double Cost => UploadSize / 1024d ;
}
