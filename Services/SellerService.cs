using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebApplicationCRUD.Data;
using WebApplicationCRUD.Models;
using Microsoft.EntityFrameworkCore;
using WebApplicationCRUD.Services.Exceptions;

namespace WebApplicationCRUD.Services {
    public class SellerService {

        private readonly WebApplicationCRUDContext _context;

        public SellerService(WebApplicationCRUDContext context) {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync() {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller obj) {
            obj.Name.TrimStart().TrimEnd();
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id) {
            return await _context.Seller.Include(obj => obj.Department)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task RemoveAsync(int id) {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e) {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Seller obj) {
            if (!await _context.Seller.AnyAsync(x => x.Id == obj.Id)) {
                throw new NotFoundException("Id not founded!");
            }
            try {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e) {
                throw new DbConcurrencyException(e.Message);
            }

        }
    }
}
