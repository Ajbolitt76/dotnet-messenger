using Messenger.Core.Requests.Abstractions;

namespace Messenger.Subscriptions.Features.PurchaseSubscriptionCommand;

public record PurchaseSubscriptionCommand(
    Guid UserId,
    string CardNumber,
    string ExpirationDate,
    string HolderName,
    string Cvv,
    uint Plan,
    uint Term) : ICommand<bool>;