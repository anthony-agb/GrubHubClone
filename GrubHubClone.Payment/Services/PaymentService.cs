using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Common.Dtos;
using GrubHubClone.Common.Dtos.MessageBus;
using GrubHubClone.Common.Enums;
using GrubHubClone.Common.Exceptions;
using GrubHubClone.Common.Models;
using GrubHubClone.Common.ServerSentEvents;
using GrubHubClone.Payment.Interfaces;
using System.Runtime.Intrinsics.X86;

namespace GrubHubClone.Payment.Services;

public class PaymentService : IPaymentService
{
    private readonly IBusClient _busClient;
    private readonly ILogger<PaymentService> _logger;
    private readonly IPaymentRepository _repository;

    public PaymentService(IBusClient busClient, ILogger<PaymentService> logger, IPaymentRepository paymentRepository)
    {
        _busClient = busClient;
        _logger = logger;
        _repository = paymentRepository;
    }

    public async Task StartPaymentProcessAsync(OrderCreatedMessage order)
    {
        await _repository.CreateAsync(new PaymentModel 
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Status = PaymentStatus.STARTED,
            CreatedTime = DateTime.UtcNow,
            UpdatedTime = DateTime.UtcNow,
            ExpirationTime = DateTime.UtcNow.AddMinutes(15)
        });

        await _busClient.PublishAsync<OrderStatusChangedMessage>(new OrderStatusChangedMessage
        {
            Id = order.Id,
            Status = OrderStatus.PROCESSING_PAYMENT
        });
    }

    public async Task<PaymentDto> GetByIdAsync(Guid id) 
    {
        var payment = await _repository.GetByIdAsync(id);

        return new PaymentDto
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            Status = payment.Status,
            CreatedTime = payment.CreatedTime,
            UpdatedTime = payment.UpdatedTime,
            ExpirationTime = payment.ExpirationTime
        };
    }

    public async Task<PaymentDto> GetByOrderIdAsync(Guid id)
    {
        var payment = await _repository.GetByOrderIdAsync(id);

        return new PaymentDto
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            Status = payment.Status,
            CreatedTime = payment.CreatedTime,
            UpdatedTime = payment.UpdatedTime,
            ExpirationTime = payment.ExpirationTime
        };
    }

    public async Task ConfirmPaymentAsync(Guid id)
    {
        var payment = await _repository.GetByOrderIdAsync(id);

        if (payment == null)
        {
            throw new ServiceException($"Payment with ID: '{id}' does not exist.");
        }

        if (payment.Status != PaymentStatus.STARTED)
        {
            throw new ServiceException($"Payment with ID: '{id}' is not in STARTED status.");
        }

        await _repository.UpdateAsync(new PaymentModel
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            Status = PaymentStatus.CONFIRMED,
            CreatedTime = payment.CreatedTime,
            UpdatedTime = DateTime.UtcNow,
            ExpirationTime = payment.ExpirationTime
        });

        await _busClient.PublishAsync<OrderStatusChangedMessage>(new OrderStatusChangedMessage
        {
            Id = id,
            Status = OrderStatus.PAYED
        });

        await _busClient.PublishAsync<SendEmailMessage>(new SendEmailMessage
        {
            Subject = $"Grubhub order: {payment.OrderId}",
            SendTo = "Bob",
            Body = "Thank you for ordering."
        });
    }
}
