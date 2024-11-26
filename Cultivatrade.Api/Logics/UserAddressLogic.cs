using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;

namespace Cultivatrade.Api.Logics
{
    public class UserAddressLogic
    {
        private readonly CultivatradeContext _context;

        public UserAddressLogic(CultivatradeContext context)
        {
            _context = context;
        }

        public List<UserAddressDTO_GET> GetUserAddress(Guid userId)
        {
            var data = (from ua in _context.UserAddresses
                        where ua.UserId == userId && ua.IsDeleted == false
                        orderby ua.DateTimeCreated descending 
                        select new UserAddressDTO_GET
                        {
                            UserAddressId = ua.UserAddressId,
                            UserId = userId,
                            Address = ua.Address,
                            DateTimeCreated = ua.DateTimeCreated,
                        }).ToList();
            return data;
        }

        public bool AddUserAddress(UserAddressDTO_POST dto)
        {
            int success = 0;

            var data = new UserAddress
            {
                UserAddressId = Guid.NewGuid(),
                UserId = dto.UserId,
                Address = dto.Address,
                DateTimeCreated = DateTime.Now,
                IsDeleted = false
            };

            _context.UserAddresses.Add(data);
            success = _context.SaveChanges();
            return success > 0;
        }

        public bool UpdateUserAddress(Guid userAddressId, UserAddressDTO_PUT dto)
        {
            int success = 0;

            var data = _context.UserAddresses.FirstOrDefault(x => x.UserAddressId == userAddressId);

            data.Address = dto.Address;

            _context.UserAddresses.Update(data);
            success = _context.SaveChanges();

            return success > 0;
        }

        public bool DeleteUserAddress(Guid userAddressId)
        {
            int success = 0;

            var data = _context.UserAddresses.FirstOrDefault(x => x.UserAddressId == userAddressId);

            data.IsDeleted = true;

            _context.UserAddresses.Update(data);
            success = _context.SaveChanges();

            return success > 0;
        }
    }
}
