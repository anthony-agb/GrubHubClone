//using GrubHubClone.Common.Exceptions;
//using GrubHubClone.Common.Models;
//using GrubHubClone.Payment.DataAccess;
//using Microsoft.EntityFrameworkCore;

//namespace GrubHubClone.Payment.DataAccess.Repositories;

//public class PaymentProcessingRepository
//{
//    private readonly DatabaseContext _db;
//    private readonly ILogger _logger;

//    public PaymentProcessingRepository(DatabaseContext db, ILogger<PaymentProcessingRepository> logger)
//    {
//        _db = db;
//        _logger = logger;
//    }

//    public async Task<Menu> CreateAsync(Menu menu)
//    {
//        try
//        {
//            var menuExists = await _db.Menus
//                .Where(m => m.Id == menu.Id)
//                .CountAsync();

//            if (menuExists == 1)
//            {
//                throw new DataAccessException($"Menu with ID: '{menu.Id}' already exists.");
//            }

//            await _db.Menus.AddAsync(menu);
//            int success = await _db.SaveChangesAsync();

//            if (success == 0)
//            {
//                throw new DataAccessException("Failed to create menu.");
//            }

//            return menu;
//        }
//        catch (DataAccessException ex)
//        {
//            _logger.LogError(ex, "Error in MenuRepository.CreateAsync.");
//            throw ex;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error in MenuRepository.CreateAsync.");
//            throw new DataAccessException("Error creating menu.", ex);
//        }
//    }

//    public async Task<List<Menu>> GetAllAsync()
//    {
//        try
//        {
//            var Menus = await _db.Menus
//                .ToListAsync();

//            return Menus;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error in MenuRepository.GetAllAsync.");
//            throw new DataAccessException("Error retrieving menus.", ex);
//        }
//    }

//    public async Task<Menu> GetByIdAsync(Guid id)
//    {
//        try
//        {
//            var menu = await _db.Menus
//                .Where(v => v.Id == id)
//                .FirstOrDefaultAsync();

//            if (menu == null)
//            {
//                throw new DataAccessException($"Menu with ID: '{id}' does not exist.");
//            }

//            return menu;
//        }
//        catch (DataAccessException ex)
//        {
//            _logger.LogError(ex, "Error in MenuRepository.GetByIdAsync.");
//            throw ex;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error in MenuRepository.GetByIdAsync.");
//            throw new DataAccessException("Error retrieving menu.", ex);
//        }
//    }

//    public async Task UpdateAsync(Menu menu)
//    {
//        try
//        {
//            var dbMenu = await _db.Menus
//                .Where(v => v.Id == menu.Id)
//                .FirstOrDefaultAsync();

//            if (dbMenu == null)
//            {
//                throw new DataAccessException($"Menu with ID: '{menu.Id}' does not exist.");
//            }

//            _db.Entry(dbMenu).State = EntityState.Detached;
//            _db.Menus.Attach(menu);
//            _db.Entry(menu).State = EntityState.Modified;
//            _db.Menus.Update(menu);

//            int success = await _db.SaveChangesAsync();

//            if (success == 0)
//            {
//                throw new DataAccessException($"Failed to remove Menu with ID: '{menu.Id}'.");
//            }
//        }
//        catch (DataAccessException ex)
//        {
//            _logger.LogError(ex, "Error in MenuRepository.UpdateAsync.");
//            throw ex;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error in MenuRepository.UpdateAsync.");
//            throw new DataAccessException("Error updating menu.", ex);
//        }
//    }

//    public async Task RemoveAsync(Guid id)
//    {
//        try
//        {
//            var menu = await _db.Menus
//                .Where(v => v.Id == id)
//                .FirstOrDefaultAsync();

//            if (menu == null)
//            {
//                throw new DataAccessException($"Menu with ID: '{id}' does not exist."); ;
//            }

//            _db.Menus.Remove(menu);
//            int success = await _db.SaveChangesAsync();

//            if (success == 0)
//            {
//                throw new DataAccessException($"Failed to remove menu with ID: '{id}'.");
//            }
//        }
//        catch (DataAccessException ex)
//        {
//            _logger.LogError(ex, "Error in MenuRepository.RemoveAsync.");
//            throw ex;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error in MenuRepository.RemoveAsync.");
//            throw new DataAccessException("Error removing menu.", ex);
//        }
//    }
//}
