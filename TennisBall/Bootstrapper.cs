using Android.App;
using TinyIoC;
using TennisBall.Data;

namespace TennisBall
{
    public static class Bootstrapper
    {
        public static void Initialize()
        {
            TinyIoCContainer.Current.Register<IDatabaseManager, DatabaseManager>();
            TinyIoCContainer.Current.Register<IMatchesRepository, MatchesRepository>();
            TinyIoCContainer.Current.AutoRegister(z => z.BaseType == typeof(Fragment));
        }
    }
}