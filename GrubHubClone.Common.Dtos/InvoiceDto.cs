namespace GrubHubClone.Common.Dtos;

public readonly record struct InvoiceDto(
    Guid Id,
    string Name,
    string Description,
    decimal TotalPrice);
