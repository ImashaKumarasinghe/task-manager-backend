using FirebaseAdmin.Auth;

namespace mini_task_manager_backend.Services
{
    public class FirebaseTokenService
    {
        public async Task<string?> VerifyTokenAsync(string authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return null;

            var idToken = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                return decodedToken.Uid;
            }
            catch
            {
                return null;
            }
        }
    }
}