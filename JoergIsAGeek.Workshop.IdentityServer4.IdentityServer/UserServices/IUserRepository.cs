namespace JoergIsAGeek.Workshop.IdentityServer4.IdentityServer.UserServices
{
    public interface IUserRepository
    {
        bool ValidateCredentials(string username, string password);

        CustomUser FindBySubjectId(string subjectId);

        CustomUser FindByUsername(string username);
    }
}
