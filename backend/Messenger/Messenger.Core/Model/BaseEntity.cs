using System.Text.Json.Serialization;
using MediatR;
using Messenger.Core.Model.Abstractions;

namespace Messenger.Core.Model;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    private List<IDomainEvent> _domainEvents = new();
    
    [JsonIgnore]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent eventItem)
    {;
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

}