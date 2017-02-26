using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using TennisBall.Data;
using TennisBall.Logic;

namespace TennisBall.Fragments
{
    public class MatchFragment : Fragment
    {
        private Toolbar toolbar;
        private IMenuItem redo;
        private IMenuItem undo;
        private IMenuItem save;
        private TextView p1Server;
        private TextView p2Server;

        private List<TextView> GamesViews;

        private TextView p1PointsView;
        private TextView p2PointsView;

        private Button addPoint1;
        private Button addPoint2;

        private TextView winner;
        private TextView winnerNameTV;

        private Match match;
        private IMatchesRepository repository;

        public MatchFragment(string player1Name, string player2Name, PlayerNumber server, IMatchesRepository repo)
        {
            match = new Match(player1Name, player2Name, server);
            GamesViews = new List<TextView>();
            repository = repo;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.match, container, false);
            toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar_bottom);

            toolbar.InflateMenu(Resource.Menu.match_menu);

            redo = toolbar.Menu.FindItem(Resource.Id.menu_redo);
            undo = toolbar.Menu.FindItem(Resource.Id.menu_undo);
            save = toolbar.Menu.FindItem(Resource.Id.menu_save);

            view.FindViewById<TextView>(Resource.Id.p1Name).Text = match.PlayerOne.Name;
            view.FindViewById<TextView>(Resource.Id.p2Name).Text = match.PlayerTwo.Name;
            view.FindViewById<TextView>(Resource.Id.namep1).Text = match.PlayerOne.Name;
            view.FindViewById<TextView>(Resource.Id.namep2).Text = match.PlayerTwo.Name;

            toolbar.MenuItemClick += Toolbar_MenuItemClick;

            addPoint1 = view.FindViewById<Button>(Resource.Id.p1Point);
            addPoint1.Click += Player1Point_Click;

            addPoint2 = view.FindViewById<Button>(Resource.Id.p2Point);
            addPoint2.Click += Player2Point_Click;

            p1Server = view.FindViewById<TextView>(Resource.Id.p1server);
            p2Server = view.FindViewById<TextView>(Resource.Id.p2server);

            p1PointsView = view.FindViewById<TextView>(Resource.Id.p1Points);
            p2PointsView = view.FindViewById<TextView>(Resource.Id.p2Points);

            winner = view.FindViewById<TextView>(Resource.Id.winner);
            winnerNameTV = view.FindViewById<TextView>(Resource.Id.winnerName);

            GamesViews.Add(view.FindViewById<TextView>(Resource.Id.game1p1));
            GamesViews.Add(view.FindViewById<TextView>(Resource.Id.game1p2));
            GamesViews.Add(view.FindViewById<TextView>(Resource.Id.game2p1));
            GamesViews.Add(view.FindViewById<TextView>(Resource.Id.game2p2));
            GamesViews.Add(view.FindViewById<TextView>(Resource.Id.game3p1));
            GamesViews.Add(view.FindViewById<TextView>(Resource.Id.game3p2));


            UpdateUI();

            return view;
        }

        private void Player1Point_Click(object sender, EventArgs e)
        {
            match.AddPoint(PlayerNumber.One);

            CheckWinner();

            UpdateUI();         
        }

        private void Player2Point_Click(object sender, EventArgs e)
        {
            match.AddPoint(PlayerNumber.Two);

            CheckWinner();

            UpdateUI();
        }

        private void CheckWinner()
        {
            string winnerName = match.GetWinner();

            if (!string.IsNullOrEmpty(winnerName))
            {
                save.SetVisible(true);
                UndoEnabled = false;
                RedoEnabled = false;
                addPoint1.Enabled = false;
                addPoint2.Enabled = false;
                addPoint1.Background.SetAlpha(125);
                addPoint2.Background.SetAlpha(125);

                winner.Visibility = ViewStates.Visible;
                
                Animation anim = new AlphaAnimation(0.0f, 1.0f);
                anim.Duration = 350;
                anim.StartOffset = 20;
                anim.RepeatMode = RepeatMode.Reverse;
                anim.RepeatCount = Animation.Infinite;
                winner.StartAnimation(anim);
                winnerNameTV.Text = winnerName;
            }
        }


        private void Toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case (Resource.Id.menu_redo):
                    match.GoForward();
                    break;
                case (Resource.Id.menu_undo):
                    match.GoBack();
                    break;
                case (Resource.Id.menu_save):
                    repository.AddMatch(match.PlayedSets,match.GetWinner());
                    (Activity as MainActivity).MatchSaved();
                    break;
            }

            UpdateUI();
        }

        private bool undoEnabled;
        public bool UndoEnabled
        {
            get
            {
                return undoEnabled;
            }
            set
            {
                undo.SetEnabled(value);
                if (value)
                    undo.SetIcon(Context.GetDrawable(Resource.Drawable.undo));
                else
                    undo.SetIcon(Context.GetDrawable(Resource.Drawable.undoDisabled));
            }
        }

        private bool redoEnabled;
        public bool RedoEnabled
        {
            get
            {
                return redoEnabled;
            }
            set
            {
                redo.SetEnabled(value);
                if (value)
                    redo.SetIcon(Context.GetDrawable(Resource.Drawable.redo));
                else
                    redo.SetIcon(Context.GetDrawable(Resource.Drawable.redoDisabled));
            }
        }


        private void UpdateServer()
        {
            if (match.CurrentServer == PlayerNumber.One)
            {
                p1Server.SetBackgroundColor(Android.Graphics.Color.ParseColor(Context.GetString(Resource.Color.green)));
                p2Server.SetBackgroundColor(Android.Graphics.Color.ParseColor(Context.GetString(Resource.Color.orange)));
            }
            else
            {
                p1Server.SetBackgroundColor(Android.Graphics.Color.ParseColor(Context.GetString(Resource.Color.orange)));
                p2Server.SetBackgroundColor(Android.Graphics.Color.ParseColor(Context.GetString(Resource.Color.green)));
            }
        }

        private void UpdatePlayedSets()
        {
            int sets = match.PlayedSets.Count();
            switch (sets)
            {
                case 0:
                    GamesViews.Single(z => z.Id == Resource.Id.game1p1).Visibility = ViewStates.Visible;
                    GamesViews.Single(z => z.Id == Resource.Id.game1p1).Text = match.PlayerOne.Games.ToString();

                    GamesViews.Single(z => z.Id == Resource.Id.game2p1).Visibility = ViewStates.Gone;
                    GamesViews.Single(z => z.Id == Resource.Id.game2p2).Visibility = ViewStates.Gone;

                    GamesViews.Single(z => z.Id == Resource.Id.game1p2).Visibility = ViewStates.Visible;
                    GamesViews.Single(z => z.Id == Resource.Id.game1p2).Text = match.PlayerTwo.Games.ToString();

                    break;
                case 1:
                    GamesViews.Single(z => z.Id == Resource.Id.game1p1).Text = match.PlayedSets[0].Player1Games;
                    GamesViews.Single(z => z.Id == Resource.Id.game1p2).Text = match.PlayedSets[0].Player2Games;


                    GamesViews.Single(z => z.Id == Resource.Id.game2p1).Visibility = ViewStates.Visible;
                    GamesViews.Single(z => z.Id == Resource.Id.game2p1).Text = match.PlayerOne.Games.ToString();

                    GamesViews.Single(z => z.Id == Resource.Id.game3p1).Visibility = ViewStates.Gone;
                    GamesViews.Single(z => z.Id == Resource.Id.game3p2).Visibility = ViewStates.Gone;

                    GamesViews.Single(z => z.Id == Resource.Id.game2p2).Visibility = ViewStates.Visible;
                    GamesViews.Single(z => z.Id == Resource.Id.game2p2).Text = match.PlayerTwo.Games.ToString();
                    break;
                case 2:
                    GamesViews.Single(z => z.Id == Resource.Id.game2p1).Text = match.PlayedSets[1].Player1Games;
                    GamesViews.Single(z => z.Id == Resource.Id.game2p2).Text = match.PlayedSets[1].Player2Games;

                    if (!match.finished)
                    {
                        GamesViews.Single(z => z.Id == Resource.Id.game3p1).Visibility = ViewStates.Visible;
                        GamesViews.Single(z => z.Id == Resource.Id.game3p2).Visibility = ViewStates.Visible;
                    }

                    GamesViews.Single(z => z.Id == Resource.Id.game3p1).Text = match.PlayerOne.Games.ToString();
                    GamesViews.Single(z => z.Id == Resource.Id.game3p2).Text = match.PlayerTwo.Games.ToString();
                    break;
                case 3:
                    GamesViews.Single(z => z.Id == Resource.Id.game3p1).Text = match.PlayedSets[2].Player1Games;
                    GamesViews.Single(z => z.Id == Resource.Id.game3p2).Text = match.PlayedSets[2].Player2Games;
                    break;
            }
        }

        public void UpdateUI()
        {
            if (!match.finished)
            {
                UndoEnabled = match.CanGoBack;
                RedoEnabled = match.CanGoForward;
            }

            p1PointsView.Text = match.PlayerOne.DisplayPoints;
            p2PointsView.Text = match.PlayerTwo.DisplayPoints;

            UpdateServer();
            UpdatePlayedSets();
        }
    }
}