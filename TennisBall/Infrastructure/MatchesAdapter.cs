using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TennisBall.Entites;
using TennisBall.Data;
using Android.Graphics;

namespace TennisBall.Infrastructure
{
    public class MatchesAdapter : BaseAdapter<SavedMatch>
    {
        IList<SavedMatch> data;
        Activity context;

        public MatchesAdapter(Activity context, IList<SavedMatch> data)
        {
            this.context = context;
            this.data = data;
        }


        public override SavedMatch this[int position]
        {
            get
            {
                return data[position];
            }
        }

        public override int Count
        {
            get
            {
                return data.Count();
            }
        }

        public override long GetItemId(int position)
        {
            return data[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.matchItem, null);
            }

            var singleData = data[position];

            view.FindViewById<TextView>(Resource.Id.p1Name).Text = singleData.Player1Name;
            view.FindViewById<TextView>(Resource.Id.p2Name).Text = singleData.Player2Name;

            view.FindViewById<TextView>(Resource.Id.p1games).Text = singleData.Player1Sets.Replace(",", "  ");
            view.FindViewById<TextView>(Resource.Id.p2games).Text = singleData.Player2Sets.Replace(",", "  ");

            if (singleData.WinnerName == singleData.Player1Name) view.FindViewById<TextView>(Resource.Id.p1Name).SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
            else view.FindViewById<TextView>(Resource.Id.p2Name).SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);


            return view;
        }
    }
}