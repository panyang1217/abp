<<<<<<< HEAD
namespace Volo.Abp.Users
{
    public static class AbpUserExtensions
    {
        public static IUserData ToAbpUserData(this IUser user)
        {
            return new UserData(
                user.Id,
                user.UserName,
                user.Email,
                user.EmailConfirmed,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.TenantId
            );
        }
    }
=======
namespace Volo.Abp.Users
{
    public static class AbpUserExtensions
    {
        public static IUserData ToAbpUserData(this IUser user)
        {
            return new UserData(
                id: user.Id,
                userName: user.UserName,
                email: user.Email,
                name: user.Name,
                surname: user.Surname,
                emailConfirmed: user.EmailConfirmed,
                phoneNumber: user.PhoneNumber,
                phoneNumberConfirmed: user.PhoneNumberConfirmed,
                tenantId: user.TenantId
            );
        }
    }
>>>>>>> upstream/master
}