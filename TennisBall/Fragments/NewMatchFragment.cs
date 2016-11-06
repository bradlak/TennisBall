using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Views.InputMethods;
using TennisBall.Logic;

namespace TennisBall.Fragments
{
    public class NewMatchFragment : Fragment
    {         
        EditText player1Name;
        EditText player2Name;

        ImageButton player1Server;
        ImageButton player2Server;

        PlayerNumber server;
        Drawable ballIcon;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            server = PlayerNumber.One;
            ballIcon = Context.GetDrawable(Resource.Drawable.tennisBall);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.newMatch, container,false);
            view.FindViewById<Button>(Resource.Id.startMatch).Click += NewMatchFragment_Click;

            player1Name = view.FindViewById<EditText>(Resource.Id.player1name);
            player2Name = view.FindViewById<EditText>(Resource.Id.player2name);

            player1Server = view.FindViewById<ImageButton>(Resource.Id.player1server);
            player1Server.Click += Player1Server_Click;

            player2Server = view.FindViewById<ImageButton>(Resource.Id.player2server);
            player2Server.Click += Player2Server_Click;

            return view;
        }

        private void Player1Server_Click(object sender, EventArgs e)
        {
            ImageButton butt = sender as ImageButton;
            butt.SetImageDrawable(ballIcon);
            player2Server.SetImageDrawable(null);
            server = PlayerNumber.One;
        }

        private void Player2Server_Click(object sender, EventArgs e)
        {
            ImageButton butt = sender as ImageButton;
            butt.SetImageDrawable(ballIcon);
            player1Server.SetImageDrawable(null);
            server = PlayerNumber.Two;
        }

        private void NewMatchFragment_Click(object sender, EventArgs e)
        {
            string name1 = string.IsNullOrEmpty(player1Name.Text) ? "Player 1" : player1Name.Text;
            string name2 = string.IsNullOrEmpty(player2Name.Text) ? "Player 2" : player2Name.Text;
            var act = (MainActivity)Activity;
            
            var imm = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
            var result = imm.HideSoftInputFromWindow(act.CurrentFocus.WindowToken, 0);

            act.StartNewMatch(name1, name2, server);            
        }
    }
}