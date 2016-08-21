using Android.Views;
using Android.Widget;
using Android.OS;
using TennisBall.Entites;
using Android.Support.V7.App;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using V7App = Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using TennisBall.Data;
using TinyIoC;
using TennisBall.Logic;
using TennisBall.Fragments;
using Android.App;
using System.Collections.Generic;

namespace TennisBall
{
    [Activity(Label = "TennisBall", MainLauncher = true, Icon = "@drawable/tennisBall",ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        IMatchesRepository repository;
        DrawerLayout drawerLayout;
        public bool MatchStarted = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            TinyIoCContainer.Current.Register<IDatabaseManager, DatabaseManager>();
            TinyIoCContainer.Current.Register<IMatchesRepository, MatchesRepository>();
            TinyIoCContainer.Current.AutoRegister(z => z.BaseType == typeof(Fragment));

            IDatabaseManager mgr = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            mgr.CreateDatabaseIfNotExist();
            mgr.CreateTable<SavedMatch>();
            repository = TinyIoCContainer.Current.Resolve<IMatchesRepository>();

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.ApplicationName);
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetDisplayShowHomeEnabled(false);

            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.OpenDrawer, Resource.String.CloseDrawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            ChangeFragment<SavedMatchesFragment>();
        }

        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.ApplicationName);
            base.OnResume();
        }

        private void ChangeFragment<T>() where T : Fragment
        {
            var ft = FragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            ft.Add(Resource.Id.AppScreen, TinyIoCContainer.Current.Resolve<T>());
            ft.Commit();
        }

        private void ChangeFragmentWithParameters<T>(Dictionary<string,object> namedParameters) where T : Fragment
        {
            var fragment = TinyIoCContainer.Current.Resolve<T>(new NamedParameterOverloads(namedParameters));
            var ft = FragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            ft.Add(Resource.Id.AppScreen, fragment);
            ft.Commit();
        }


        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_new_match):
                    if (MatchStarted)
                    {
                        V7App.AlertDialog.Builder alert = new V7App.AlertDialog.Builder(this);
                        alert.SetTitle(Resource.String.Confirmation);
                        alert.SetMessage(Resource.String.NotFinishedMatchQuestion);
                        alert.SetPositiveButton("Yes", (s, ee) =>
                         {
                             ChangeFragment<NewMatchFragment>();
                         });
                        alert.SetNegativeButton("No", (s, ee) => { });
                        alert.Show();
                    }
                    else
                    {
                        ChangeFragment<NewMatchFragment>();
                    }

                    break;
                case (Resource.Id.nav_saved):
                    ChangeFragment<SavedMatchesFragment>();
                    break;
            }
            drawerLayout.CloseDrawers();
        }

        public void StartNewMatch(string player1Name, string player2Name, PlayerNumber server)
        {
           var parameters = new Dictionary<string, object>()
                    { { "player1Name",player1Name },
                      { "player2Name",player2Name },
                      { "server"     ,server } };

            ChangeFragmentWithParameters<MatchFragment>(parameters);
            MatchStarted = true;
        }

        public void MatchSaved()
        {
            Toast.MakeText(this, Resource.String.SavedMatch, ToastLength.Short).Show();
            ChangeFragment<SavedMatchesFragment>();
            MatchStarted = false;
        }

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
       

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_settings:
                    Toast.MakeText(this, Resource.String.NotAvailiable, ToastLength.Short).Show();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnBackPressed()
        {
            
        }


    }
}

