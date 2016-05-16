using System;
using System.Linq;
using System.Windows.Forms;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using LeagueSharp.Common;
using SharpDX;
using EloBuddy.SDK;
using EloBuddy.SDK.Notifications;
using CheckBox = EloBuddy.SDK.Menu.Values.CheckBox;
using ComboBox = EloBuddy.SDK.Menu.Values.ComboBox;
using MainMenu = EloBuddy.SDK.Menu.MainMenu;

namespace PortAIO.Utility.WhiteFeeder
{
    class Program
    {
        private static readonly AIHeroClient Player = ObjectManager.Player;
        private static EloBuddy.SDK.Menu.Menu config;

        private static readonly bool[] CheckForBoughtItem = { false, false, false, false, false };

        private static bool point1Reached;
        private static bool point2Reached;

        private static SpellSlot ghost;

        private static bool isDead;
        private static bool saidDeadStuff;

        private static float lastChat;
        private static float lastLaugh;

        public static void Main()
        {
            if (Game.Mode == GameMode.Running)
            {
                Game_OnGameLoad(new EventArgs());
            }
            else
            {
                CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            }
        }

        


        static void Game_OnGameLoad(EventArgs args)
        {
            Notifications.Show(new SimpleNotification("WhiteFeeder","Loading White Feeder...."),300);


            config = MainMenu.AddMenu("White Feeder", "whitefeeder");
            config.Add("root.shouldfeed", new CheckBox("Feeding Enabled", false));
            config.Add("root.feedmode",
                new ComboBox("Feeding Mode:", 0,
                    new string[]
                    {
                        "Closest Enemy", "Bottom Lane", "Middle Lane", "Top Lane", "Wait at Dragon", "Wait at Baron",
                        "Most Fed", "Highest Carrying Potential"
                    }));

            config.Add("root.defaultto",
                new ComboBox("Default To:", 0, new string[] {"Bottom Lane", "Top Lane", "Middle Lane"}));
            config.AddSeparator();

            config.Add("root.chat", new CheckBox("Chat at Baron/Dragon", false));
            config.Add("root.chat.delay", new Slider("Baron/Dragon Chat Delay", 2000, 0, 10000));
            config.Add("root.chat2", new CheckBox("Chat on Death"));
            config.AddSeparator();
            config.Add("root.items", new CheckBox("Buy speed items"));
            config.AddSeparator();
            config.Add("root.ghost", new CheckBox("Use Ghost", false));

            ghost = Player.GetSpellSlot("summonerhaste");

            Chat.OnInput += Game_OnInput;
            Game.OnUpdate += Game_OnUpdate;
            CustomEvents.Game.OnGameEnd += Game_OnGameEnd;

            Notifications.Show(new SimpleNotification("WhiteFeeder", Player.Team == GameObjectTeam.Chaos ? "White Feeder: Team Chaos" : "White Feeder: Team Order"),300);
        }

        static void Game_OnGameEnd(EventArgs args)
        {
            Chat.Say("/all Good game lads! :)");

        }

        static void Game_OnUpdate(EventArgs args)
        {
            var feedmode = config["root.feedmode"].Cast<ComboBox>().SelectedIndex;
            var defaultto = config["root.defaultto"].Cast<ComboBox>().SelectedIndex;

            Vector3 botTurningPoint1 = new Vector3(12124, 1726, 52);
            Vector3 botTurningPoint2 = new Vector3(13502, 3494, 51);

            Vector3 topTurningPoint1 = new Vector3(1454, 11764, 53);
            Vector3 topTurningPoint2 = new Vector3(3170, 13632, 53);

            Vector3 dragon = new Vector3(10064, 4646, -71);
            Vector3 baron = new Vector3(4964, 10380, -71);

            Vector3 chaosUniversal = new Vector3(14287f, 14383f, 172f);
            Vector3 orderUniversal = new Vector3(417f, 469f, 182f);

            string[] msgList = { "wat", "how?", "What?", "how did you manage to do that?", "mate..", "-_-",
                "why?", "lag", "laaaaag", "oh my god this lag is unreal", "rito pls 500 ping", "god bless my ping",
                "if my ping was my iq i'd be smarter than einstein", "what's up with this lag?", "is the server lagging again?",
            "i call black magic" };

            if (isDead)
            {
                if (!saidDeadStuff && config["root.chat2"].Cast<CheckBox>().CurrentValue)
                {
                    Random r = new Random();
                    Chat.Say("/all " + msgList[r.Next(0, 14)]);
                    saidDeadStuff = true;
                }
            }

            if (Player.IsDead)
            {
                isDead = true;
                point1Reached = false;
                point2Reached = false;
            }
            else
            {
                isDead = false;
                saidDeadStuff = false;

                if (Player.InFountain())
                {
                    point1Reached = false;
                    point2Reached = false;
                }

                if (Player.Distance(botTurningPoint1) <= 300 || Player.Distance(topTurningPoint1) <= 300)
                    point1Reached = true;
                if (Player.Distance(botTurningPoint2) <= 300 || Player.Distance(topTurningPoint2) <= 300)
                    point2Reached = true;
            }

            if (!config["root.shouldfeed"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if ((lastLaugh == 0 || lastLaugh < Game.Time) && config["root.laugh"].Cast<CheckBox>().CurrentValue)
            {
                lastLaugh = Game.Time + config["root.laugh.delay"].Cast<Slider>().CurrentValue;
                Chat.Say("/laugh");
            }

            if (ghost != SpellSlot.Unknown
                && Player.Spellbook.CanUseSpell(ghost) == SpellState.Ready
                && config["root.ghost"].Cast<CheckBox>().CurrentValue
                && Player.InFountain())
            {
                Player.Spellbook.CastSpell(ghost);
            }

            if (config["root.items"].Cast<CheckBox>().CurrentValue && Player.InShop())
            {
                if (Player.Gold >= 325
                    && !CheckForBoughtItem[0])
                {
                    Shop.BuyItem(ItemId.Boots_of_Speed);
                    CheckForBoughtItem[0] = true;
                }
                if (Player.Gold >= 475
                    && CheckForBoughtItem[0]
                    && !CheckForBoughtItem[1])
                {
                    Shop.BuyItem(ItemId.Boots_of_Mobility);
                    CheckForBoughtItem[1] = true;
                }
                /*if (Player.Gold >= 475
                    && CheckForBoughtItem[1]
                    && !CheckForBoughtItem[2])
                {
                    Shop.BuyItem(ItemId.Boots_of_Mobility_Enchantment_Homeguard);
                    CheckForBoughtItem[2] = true;
                }*/
                if (Player.Gold >= 435
                    && CheckForBoughtItem[2]
                    && !CheckForBoughtItem[3])
                {
                    Shop.BuyItem(ItemId.Amplifying_Tome);
                    CheckForBoughtItem[3] = true;
                }
                if (Player.Gold >= (850 - 435)
                    && CheckForBoughtItem[3]
                    && !CheckForBoughtItem[4])
                {
                    Shop.BuyItem(ItemId.Aether_Wisp);
                    CheckForBoughtItem[4] = true;
                }
                if (Player.Gold > 1100
                    && CheckForBoughtItem[4])
                {
                    Shop.BuyItem(ItemId.Zeal);
                }

            }


            switch (feedmode)
            {
                case 0:
                    if (HeroManager.Enemies.Where(x => x.IsValidTarget() && !x.IsDead).OrderBy(x => x.Distance(Player.Position)).FirstOrDefault().IsValidTarget())
                    {
                        EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo,
                            HeroManager.Enemies.Where(x => x.IsValidTarget() && !x.IsDead).OrderBy(x => x.Distance(Player.Position)).FirstOrDefault());
                    }
                    else
                    {
                        switch (defaultto)
                        {
                            case 0:
                                {
                                    if (Player.Team == GameObjectTeam.Order)
                                    {
                                        if (!point1Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint1);
                                        }
                                        else if (!point2Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint2);
                                        }
                                        else if (point2Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, chaosUniversal);
                                        }
                                    }
                                    else
                                    {
                                        if (!point2Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint2);
                                        }
                                        else if (!point1Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint1);
                                        }
                                        else if (point2Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, orderUniversal);
                                        }
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (Player.Team == GameObjectTeam.Order)
                                    {
                                        if (!point1Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint1);
                                        }
                                        else if (!point2Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint2);
                                        }
                                        else if (point2Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, chaosUniversal);
                                        }
                                    }
                                    else
                                    {
                                        if (!point2Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint2);
                                        }
                                        else if (!point1Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint1);
                                        }
                                        else if (point2Reached)
                                        {
                                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, orderUniversal);
                                        }
                                    }
                                }
                                break;
                            case 2:
                                {
                                    EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo,
                                        Player.Team == GameObjectTeam.Order ? chaosUniversal : orderUniversal);
                                }
                                break;
                            default:
                                Console.WriteLine(@"");
                                break;
                        }
                    }
                    break;
                case 1:
                    {
                        if (Player.Team == GameObjectTeam.Order)
                        {
                            if (!point1Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint1);
                            }
                            else if (!point2Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint2);
                            }
                            else if (point2Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, chaosUniversal);
                            }
                        }
                        else
                        {
                            if (!point2Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint2);
                            }
                            else if (!point1Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint1);
                            }
                            else if (point2Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, orderUniversal);
                            }

                        }
                    }
                    break;
                case 2:
                    {
                        EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo,
                            Player.Team == GameObjectTeam.Order ? chaosUniversal : orderUniversal);
                    }
                    break;
                case 3:
                    {
                        if (Player.Team == GameObjectTeam.Order)
                        {
                            if (!point1Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint1);
                            }
                            else if (!point2Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint2);
                            }
                            else if (point2Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, chaosUniversal);
                            }
                        }
                        else
                        {
                            if (!point2Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint2);
                            }
                            else if (!point1Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint1);
                            }
                            else if (point2Reached)
                            {
                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, orderUniversal);
                            }
                        }
                    }
                    break;
                case 4:
                    {
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        if ((lastChat == 0 || lastChat < Game.Time) && config["root.chat"].Cast<CheckBox>().CurrentValue
                            && Player.Distance(dragon) <= 300)
                        {
                            lastChat = Game.Time + config["root.chat.delay"].Cast<Slider>().CurrentValue;
                            Chat.Say("/all Come to dragon!");
                        }
                        EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, dragon);

                    }
                    break;
                case 5:
                    {
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        if ((lastChat == 0 || lastChat < Game.Time) && config["root.chat"].Cast<CheckBox>().CurrentValue
                            && Player.Distance(baron) <= 300)
                        {
                            lastChat = Game.Time + config["root.chat.delay"].Cast<Slider>().CurrentValue;
                            Chat.Say("/all Come to baron!");
                        }
                        EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, baron);
                    }
                    break;
                case 6:
                    {
                        if (HeroManager.Enemies.Where(x => x.IsValidTarget() && !x.IsDead).OrderBy(x => x.ChampionsKilled).LastOrDefault().IsValidTarget())
                        {
                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo,
                                HeroManager.Enemies.Where(x => x.IsValidTarget() && !x.IsDead).OrderBy(x => x.ChampionsKilled).LastOrDefault());
                        }
                        else
                        {
                            switch (defaultto)
                            {
                                case 0:
                                    {
                                        if (Player.Team == GameObjectTeam.Order)
                                        {
                                            if (!point1Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint1);
                                            }
                                            else if (!point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint2);
                                            }
                                            else if (point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, chaosUniversal);
                                            }
                                        }
                                        else
                                        {
                                            if (!point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint2);
                                            }
                                            else if (!point1Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint1);
                                            }
                                            else if (point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, orderUniversal);
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    {
                                        if (Player.Team == GameObjectTeam.Order)
                                        {
                                            if (!point1Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint1);
                                            }
                                            else if (!point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint2);
                                            }
                                            else if (point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, chaosUniversal);
                                            }
                                        }
                                        else
                                        {
                                            if (!point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint2);
                                            }
                                            else if (!point1Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint1);
                                            }
                                            else if (point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, orderUniversal);
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    {
                                        EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo,
                                            Player.Team == GameObjectTeam.Order ? chaosUniversal : orderUniversal);
                                    }
                                    break;
                                default:
                                    Console.WriteLine(@"");
                                    break;
                            }

                        }
                    }
                    break;
                case 7:
                    {
                        if (HeroManager.Enemies.FirstOrDefault(x => x.IsValidTarget() && !x.IsDead
                                                                    && (x.ChampionName == "Katarina" || x.ChampionName == "Fiora" || x.ChampionName == "Jinx" || x.ChampionName == "Vayne")).IsValidTarget())
                        {
                            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo,
                                HeroManager.Enemies.FirstOrDefault(x => x.IsValidTarget() && !x.IsDead
                                                                        && (x.ChampionName == "Katarina" || x.ChampionName == "Fiora" || x.ChampionName == "Jinx" || x.ChampionName == "Vayne")));
                        }
                        else
                        {
                            switch (defaultto)
                            {
                                case 0:
                                    {
                                        if (Player.Team == GameObjectTeam.Order)
                                        {
                                            if (!point1Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint1);
                                            }
                                            else if (!point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint2);
                                            }
                                            else if (point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, chaosUniversal);
                                            }
                                        }
                                        else
                                        {
                                            if (!point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint2);
                                            }
                                            else if (!point1Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, botTurningPoint1);
                                            }
                                            else if (point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, orderUniversal);
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    {
                                        if (Player.Team == GameObjectTeam.Order)
                                        {
                                            if (!point1Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint1);
                                            }
                                            else if (!point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint2);
                                            }
                                            else if (point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, chaosUniversal);
                                            }
                                        }
                                        else
                                        {
                                            if (!point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint2);
                                            }
                                            else if (!point1Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, topTurningPoint1);
                                            }
                                            else if (point2Reached)
                                            {
                                                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, orderUniversal);
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    {
                                        EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Player.Team == GameObjectTeam.Order ? chaosUniversal : orderUniversal);
                                    }
                                    break;
                                default:
                                    Console.WriteLine(@"");
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    Console.WriteLine(@"");
                    break;
            }
        }

        static void Game_OnInput(ChatInputEventArgs args)
        {
            if (args.Input != "/getpos")
            {
                return;
            }
            args.Process = false;
            Clipboard.SetText(Player.Position.ToString());
            Notifications.Show(new SimpleNotification("WhiteFeeder", "Copied position to clipboard."),500);
        }

    }
}