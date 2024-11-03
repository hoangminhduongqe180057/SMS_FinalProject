using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Services
{
    public class HostelRoomRepository : IHostelRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public HostelRoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<HostelRoom> AddAsync(HostelRoom Hostelroom)
        {
            _context.HostelRooms.Add(Hostelroom);
            await _context.SaveChangesAsync();
            return Hostelroom;
        }

        public async Task<HostelRoom> DeleteAsync(int HostelroomId)
        {
            var data = await _context.HostelRooms.Where(x => x.Id == HostelroomId).FirstOrDefaultAsync();
            if (data == null) return null;
            _context.HostelRooms.Remove(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<HostelRoom>> GetAllAsync()
        {
            var data = await _context.HostelRooms.Include(s => s.RoomType).Include(h => h.Hostel).ToListAsync();

            return data;
        }

        public async Task<HostelRoom> GetByIdAsync(int HostelroomId)
        {
            var data = await _context.HostelRooms.Include(s => s.RoomType).Include(h => h.Hostel).FirstOrDefaultAsync(s => s.Id == HostelroomId);
            if (data == null) return null;
            return data;
        }

        public async Task<PaginationModel<HostelRoom>> GetPagedHostelRoomsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.HostelRooms.CountAsync();
            var hotelrooms = await _context.HostelRooms
                .Include(s => s.RoomType)
                .Include(h => h.Hostel)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<HostelRoom>
            {
                Items = hotelrooms,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<HostelRoom> UpdateAsync(HostelRoom Hostelroom)
        {
            if (Hostelroom == null) return null;

            var data = _context.HostelRooms.Update(Hostelroom).Entity;
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<int> GetTotalHostelRoomsCountAsync()
        {
            return await _context.HostelRooms.CountAsync();
        }
    }
}
