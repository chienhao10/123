﻿#region

using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using LeagueSharp.Common;
using SharpDX;
using EloBuddy.SDK.Menu;
using Spell = LeagueSharp.Common.Spell;
using Utility = LeagueSharp.Common.Utility;
using EloBuddy.SDK.Menu.Values;

#endregion

namespace SimpleSion
{
    internal class Program
    {
        private static Menu Menu;
        public static Menu comboMenu, rMenu;

        public static Spell Q;
        public static Spell E;

        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        public static Vector2 QCastPos = new Vector2();


        public static void Game_OnGameLoad()
        {
            if (ObjectManager.Player.BaseSkinName != "Sion")
            {
                return;
            }

            //Spells
            Q = new Spell(SpellSlot.Q, 1050);
            Q.SetSkillshot(0.6f, 100f, float.MaxValue, false, SkillshotType.SkillshotLine);
            Q.SetCharged("SionQ", "SionQ", 500, 720, 0.5f);

            E = new Spell(SpellSlot.E, 800);
            E.SetSkillshot(0.25f, 80f, 1800, false, SkillshotType.SkillshotLine);


            Menu = MainMenu.AddMenu("Simple Sion", "Sion");

            comboMenu = Menu.AddSubMenu("Combo", "Combo");
            comboMenu.Add("UseQCombo", new CheckBox("Use Q"));
            comboMenu.Add("UseWCombo", new CheckBox("Use W"));
            comboMenu.Add("UseECombo", new CheckBox("Use E"));


            rMenu = Menu.AddSubMenu("R", "R settings");
            rMenu.Add("AntiCamLock", new CheckBox("Avoid locking camera"));
            rMenu.Add("MoveToMouse", new CheckBox("Move to mouse (Exploit)", false));
            rMenu.AddLabel("This can get you banned");

            Chat.Print("Sion Loaded!");
            Game.OnUpdate += Game_OnGameUpdate;
            Game.OnProcessPacket += Game_OnGameProcessPacket;
            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnProcessSpellCast += ObjAiHeroOnOnProcessSpellCast;
        }

        public static bool getCheckBoxItem(Menu m, string item)
        {
            return m[item].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderItem(Menu m, string item)
        {
            return m[item].Cast<Slider>().CurrentValue;
        }

        public static bool getKeyBindItem(Menu m, string item)
        {
            return m[item].Cast<KeyBind>().CurrentValue;
        }

        public static int getBoxItem(Menu m, string item)
        {
            return m[item].Cast<ComboBox>().CurrentValue;
        }

        private static void ObjAiHeroOnOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "SionQ")
            {
                QCastPos = args.End.To2D();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            Utility.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.White);
        }

        private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if (getCheckBoxItem(rMenu, "AntiCamLock") && args.PacketData[0] == 0x83 && args.PacketData[7] == 0x47 && args.PacketData[8] == 0x47)
            {
                args.Process = false;
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            //Casting R
            if (ObjectManager.Player.HasBuff("SionR"))
            {
                if (getCheckBoxItem(rMenu, "MoveToMouse"))
                {
                    var p = ObjectManager.Player.Position.LSTo2D().LSExtend(Game.CursorPos.LSTo2D(), 500);
                    EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, p.To3D());
                }
                return;
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                var qTarget = TargetSelector.GetTarget(
                    !Q.IsCharging ? Q.ChargedMaxRange / 2 : Q.ChargedMaxRange, DamageType.Physical);

                var eTarget = TargetSelector.GetTarget(E.Range, DamageType.Physical);

                if (qTarget != null && getCheckBoxItem(comboMenu, "UseQCombo"))
                {
                    if (Q.IsCharging)
                    {
                        var start = ObjectManager.Player.ServerPosition.To2D();
                        var end = start.Extend(QCastPos, Q.Range);
                        var direction = (end - start).Normalized();
                        var normal = direction.Perpendicular();

                        var points = new List<Vector2>();
                        var hitBox = qTarget.BoundingRadius;
                        points.Add(start + normal * (Q.Width + hitBox));
                        points.Add(start - normal * (Q.Width + hitBox));
                        points.Add(end + Q.ChargedMaxRange * direction - normal * (Q.Width + hitBox));
                        points.Add(end + Q.ChargedMaxRange * direction + normal * (Q.Width + hitBox));

                        for (var i = 0; i <= points.Count - 1; i++)
                        {
                            var A = points[i];
                            var B = points[i == points.Count - 1 ? 0 : i + 1];

                            if (qTarget.ServerPosition.To2D().Distance(A, B, true, true) < 50 * 50)
                            {
                                Q.Cast(qTarget, true);
                            }
                        }
                        return;
                    }

                    if (Q.IsReady())
                    {
                        Q.StartCharging(qTarget.ServerPosition);
                    }
                }

                if (qTarget != null && getCheckBoxItem(comboMenu, "UseWCombo"))
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, ObjectManager.Player);
                }

                if (eTarget != null && getCheckBoxItem(comboMenu, "UseECombo"))
                {
                    E.Cast(eTarget);
                }
            }
        }
    }
}
