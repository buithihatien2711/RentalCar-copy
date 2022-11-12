using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentalCar.Model.Models;

namespace RentalCar.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
        }

        public Role? GetRoleById(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Role>? GetRolesOfUser(int idUser)
        {
            var user = GetUserById(idUser);
            if (user == null) return null;

            List<RoleUser> roleUsers = _context.RoleUsers.Include(ru => ru.User).Include(ru => ru.Role)
                                        .Where(u => u.UserId == idUser).ToList();

            List<Role> roles = new List<Role>();
            foreach (var roleUser in roleUsers)
            {
                roles.Add(roleUser.Role);
            }
            return roles;
        }

        public User? GetUserById(int id)
        {
            return _context.Users.Include(u => u.License).FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users.Include(u => u.License).FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<User>? GetUsers()
        {
            return _context.Users.Include(u => u.License).ToList();
        }

        public IEnumerable<User>? GetUsersByRole(int idRole)
        {
            var role = _context.RoleUsers.FirstOrDefault(r => r.RoleId == idRole);

            if (role == null) return null;

            List<RoleUser> roleUsers = _context.RoleUsers.Include(ru => ru.User).Include(ru => ru.Role)
                                        .Where(r => r.RoleId == idRole).ToList();

            List <User> users = new List<User>();

            foreach (var roleUser in roleUsers)
            {
                users.Add(roleUser.User);
            }

            return users;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }

        public void UpdateUser(string username, User user)
        {
            var existUser = GetUserByUsername(username);
            if(user == null) return;
            existUser = user;
        }
    }
}