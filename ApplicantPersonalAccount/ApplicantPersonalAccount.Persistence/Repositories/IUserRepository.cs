﻿using ApplicantPersonalAccount.Common.Models;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;

namespace ApplicantPersonalAccount.Persistence.Repositories
{
    public interface IUserRepository
    {
        public void AddUser(UserEntity user);
        public Task<bool> EmailAndPhoneIsAvailable(string email, string phone);
        public void AddRefreshToken(RefreshTokenEntity refreshToken);
        public Task SaveChanges();
        public Task<UserEntity> GetUsersByCredentials(string email, string password);
        public Task<RefreshTokenEntity> GetRefreshToken(string token);
        public Task<UserEntity> GetUserById(Guid id);
        public Task EditInfoForEvents(EditApplicantInfoForEventsModel editedInfo, Guid userId);
        public Task<InfoForEventsEntity> GetInfoForEvents(Guid userId);
    }
}
