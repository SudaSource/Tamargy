using Core.Data.Models;
using Core.Lib.SDK;

namespace Core.UI.Web.Utilites
{
    public static class UserControle
    {
        public static User GetUser(long id)
        {
            SDK sdk = new SDK();
            return sdk.UsersManager.GetSingleUserStatic(id);
        }

    }
}