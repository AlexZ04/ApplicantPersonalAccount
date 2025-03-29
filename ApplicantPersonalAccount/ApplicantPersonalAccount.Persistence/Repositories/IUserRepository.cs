using ApplicantPersonalAccount.Common.Models;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;

namespace ApplicantPersonalAccount.Persistence.Repositories
{
    public interface IUserRepository
    {
        public void AddUser(UserEntity user);
        public Task<bool> EmailIsAvailable(string email);
        public void AddRefreshToken(RefreshTokenEntity refreshToken);
        public Task SaveChanges();
    }
}
