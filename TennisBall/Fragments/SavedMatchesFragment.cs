using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using TennisBall.Infrastructure;
using TennisBall.Data;
using static Android.Widget.AdapterView;

namespace TennisBall.Fragments
{
    public class SavedMatchesFragment : Fragment, IOnItemLongClickListener
    {
        ListView matchesList;
        IMatchesRepository repository;

        public SavedMatchesFragment(IMatchesRepository repo)
        {
            this.repository = repo;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.savedMatches, container, false);

            matchesList = view.FindViewById<ListView>(Resource.Id.savedMatches);
            matchesList.EmptyView = view.FindViewById<TextView>(Resource.Id.emptyList);

            matchesList.OnItemLongClickListener = this;

            matchesList.Adapter = new MatchesAdapter(Activity, repository.GetAllMatches().ToList());

            return view;
        }

        public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
            alert.SetTitle(Resource.String.Confirmation);
            alert.SetMessage(Resource.String.DeleteMatchQuestion);
            alert.SetPositiveButton("Delete", (senderAlert, args) => {
                repository.RemoveMatch(Convert.ToInt32(id));
                FillList();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();
            dialog.Show();

            return true;
        }

        public void FillList()
        {
            matchesList.Adapter = new MatchesAdapter(Activity, repository.GetAllMatches().ToList());
        }
    }
}