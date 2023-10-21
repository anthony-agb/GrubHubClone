using GrubHubClone.Common.Dtos;

namespace GrubHubClone.Order.Interfaces
{
    public interface IOrderService
    {
        Task<InvoiceDto> CreateAsync(InvoiceDto order);
        Task<List<InvoiceDto>> GetAllAsync();
    }
}