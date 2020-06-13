using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Core.Data.Models;
using Core.Data.Types;
using Core.Lib.Core;
using Core.Lib.SDK.ViewModels;
using Core.Lib.Utilites;

namespace Core.Lib.SDK.Managers
{
    public class UsersManager
    {
        private Worker _manager;
        public UsersManager(Worker _worker)
        {
            _manager = _worker;
        }

        /// <summary>
        /// Get user from the database.
        /// </summary>
        /// <param name="name">user name</param>
        /// <param name="pass">user password</param>
        /// <returns></returns>
        public async Task<User> Login(string name) => await _manager.Users.SingleOrDefault
            (x => (x.Email == name || x.Phone == name));

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="user">user obj</param>
        /// <returns></returns>
        public async Task<bool> AddUser(User user, long creatorId)
        {
            //Check if the user is exist first.
            var isExist = await _manager.Users.SingleOrDefault(x =>
                x.Email == user.Email || x.Phone == user.Phone);
            var creator = await _manager.Users.GetSingle(creatorId);
            if (isExist == null && creator != null)
            {
                //Add the user.  
                user.Serial = Guid.NewGuid().ToString();
                user.CreateDate = DateTime.Now;
                user.SetNewPassword = true;
                //user.CreatedBy = creator;
                _manager.Users.Add(user);
            }
            //will return true if it has been added.
            //await _manager.Complete();
            ////Create Account
            //var us = await _manager.Users.SingleOrDefault(x =>
            //    x.UserName == user.UserName || x.Phone == user.Phone);
            //var creator = await _manager.Users.GetSingle(creatorId);

            //if (us!= null && creator != null)
            //{
            //    var account = await _manager.Accounts.SingleOrDefault(c => c.Owner.Id == us.Id);
            //    if (account == null)
            //    {
            //        _manager.Accounts.Add(new Account
            //        {
            //            AccountType = us.Role, CreateDate = DateTime.Now, CreatedBy = creator, Owner = us, Serial = Guid.NewGuid()
            //        });
            //    }
            //}
            return await _manager.Complete();
        }

        /// <summary>
        /// Flag user as Deleted, It can be recovered.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUser(User user)
        {
            //Check the user.
            var isExist = await _manager.Users.SingleOrDefault(x =>
                x.Email == user.Email || x.Phone == user.Phone);
            if (isExist != null)
            {
                //Flag as Deleted user.
                user.IsDeleted = true;
                _manager.Users.Update(user);
            }
            return await _manager.Complete();
        }

        /// <summary>
        /// Update user info.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(User user)
        {
            var isExist = await _manager.Users.SingleOrDefault(x =>
                (x.Email == user.Email || x.Phone == user.Phone) && x.Id != user.Id);
            if (isExist == null)
            {
                var newUser = await _manager.Users.GetSingle(user.Id);
                if (newUser != null)
                {
                    newUser.Name = user.Name;
                    newUser.Phone = user.Phone;
                    newUser.Email = user.Email;
                    _manager.Users.Update(newUser);
                }
            }
            return await _manager.Complete();
        }

        /// <summary>
        /// Add new password to the user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> SetPassword(User user)
        {
            //check the user first.
            var isExist = await _manager.Users.SingleOrDefault(x =>
                x.Email == user.Email || x.Phone == user.Phone);
            if (isExist != null)
            {
                //set password.
                isExist.Password = EncryptionManager.Encrypt(user.Password);
                isExist.SetNewPassword = false;
                _manager.Users.Update(isExist);
            }
            return await _manager.Complete();
        }

        //GET User for layout page from cookies 
        public User GetSingleUserStatic(long id) => _manager.Users.Get(id);

        public async Task<User> GetSingleUser(long id)
        {
            var user = await _manager.Users.GetSingle(id);
            // user.Password = "";
            return user;
        }


        public async Task<bool> ConfiremPassword(User us, string pass)
        {
            //Encrypting the password first.
            pass = EncryptionManager.Encrypt(pass);
            var user = await _manager.Users.SingleOrDefault(x => x.Id == us.Id);
            return ((user != null) && user.Password == pass);
        }

        public async Task<List<UserVM>> GetUsers(int role)
        {
            var us = await _manager.Users.Find(x => x.Role == (Role)role);
            var users = await (from s in _manager._context.Users
                               where
                                   !s.IsDeleted && s.Role == (Role)role
                               select new UserVM
                               {
                                   Id = s.Id,
                                   CreateDate = s.CreateDate,
                                   Gender = s.Gender,
                                   Img = s.Img,
                                   IsActive = s.IsActive,
                                   Email = s.Email,
                                   Phone = s.Phone,
                                   Name = s.Name,
                                   Serial = s.Serial
                               }).ToListAsync();
            List<User> reList = new List<User>();
            //foreach (var user in users)
            //{
            //    user.Password = "";
            //    user.Transactions = null;
            //    reList.Add(user);
            //}

            return users.OrderByDescending(x => x.Name).ToList();
        }

        public async Task<bool> ResetPassword(User user)
        {
            user.Password = "";
            user.SetNewPassword = true;
            _manager.Users.Update(user);
            return await _manager.Complete();
        }

        public async Task<bool> UpdateImage(byte[] image, long getUserId)
        {
            try
            {
                var user = await _manager._context.Users.FindAsync(getUserId);
                if (user != null)
                {
                    user.Img = image;
                    _manager._context.Entry(user).State = EntityState.Modified;
                }
            }
            catch (Exception e)
            {

            }

            return await _manager.Complete();
        }

        public async Task<bool> UpdatePass(string pass, string newPass, long getUserId)
        {

            try
            {
                var user = await _manager._context.Users.FindAsync(getUserId);
                if (user != null)
                {
                    if (user.Password == EncryptionManager.Encrypt(pass))
                    {
                        user.Password = EncryptionManager.Encrypt(newPass);
                    }

                    _manager._context.Entry(user).State = EntityState.Modified;
                }
            }
            catch (Exception e)
            {

            }

            return await _manager.Complete();
        }

        public async Task<bool> UpdateProfile(string name, string phone, string email, Gender gender, long userId)
        {
            try
            {
                var user = await _manager._context.Users.FindAsync(userId);

                if (user != null)
                {
                    var emailIsUsed = await _manager._context.Users.Where(x => x.Email == email && x.Id != userId).ToListAsync();
                    if (emailIsUsed.Count == 0)
                    {
                        user.Name = name;
                        user.Email = email;
                        user.Gender = gender;
                        _manager._context.Entry(user).State = EntityState.Modified;
                    }

                }
            }
            catch (Exception e)
            {

            }

            return await _manager.Complete();
        }

        public async Task<bool> Register(string name, string phone, string email, string pass, Gender gender, Role role)
        {
            try
            {
                var old = await _manager.Users.SingleOrDefault(x => x.Phone == phone || x.Email == email);
                if (old != null)
                {
                    return false;
                }
                else
                {
                    _manager.Users.Add(new User
                    {
                        CreateDate = DateTime.Now,
                        Email = email,
                        Gender = gender,
                        IsActive = true,
                        Password = EncryptionManager.Encrypt(pass),
                        Name = name,
                        Phone = phone,
                        Role = role,
                        Serial = Guid.NewGuid().ToString(),
                    });
                }
            }
            catch (Exception e)
            {


            }
            return await _manager.Complete();
        }
    }
}