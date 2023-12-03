using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Communication.Email;
using Azure;
using System.Text;
using System.Text.Json;
using GrubHubClone.Common.Dtos.MessageBus;

namespace GrubHubClone.Azure.Notification;

public class SendEmail
{
    private readonly ILogger<SendEmail> _logger;

    public SendEmail(ILogger<SendEmail> logger)
    {
        _logger = logger;
    }

    [Function(nameof(SendEmail))]
    public async Task Run([ServiceBusTrigger("send-email", Connection = "AzureServiceBus")] ServiceBusReceivedMessage message)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);

        if (message.Body == null) 
        {
            _logger.LogInformation("Message with ID: {id} was not send", message.MessageId);
            return;
        }

        string decodedMessageBody = Encoding.UTF8.GetString(message.Body);

        var msg = JsonSerializer.Deserialize<SendEmailMessage>(decodedMessageBody);

        string connectionString = Environment.GetEnvironmentVariable("AzureEmailService") ?? throw new NullReferenceException("The Azure Email Service connectionstring is null.");
        var emailClient = new EmailClient(connectionString);

        EmailSendOperation emailSendOperation = await emailClient.SendAsync(
            WaitUntil.Completed,
            senderAddress: "DoNotReply@41c9ed01-c8a9-435a-adf6-fcd3bcc3e21f.azurecomm.net",
            recipientAddress: "a.bergrok@student.fontys.nl",
            subject: msg.Subject,
            htmlContent: $"<html><h1>{msg.Body}</h1l></html>");
    }
}