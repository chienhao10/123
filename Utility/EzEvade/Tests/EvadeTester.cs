﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Color = System.Drawing.Color;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EzEvade;
using Microsoft.SqlServer.Server;
using SharpDX;
<<<<<<< HEAD
=======
using LeagueSharp.Common;
>>>>>>> origin/master

namespace ezEvade
{
    class EvadeTester
    {
        public static Menu menu;
        public static Menu testMenu;

        private static AIHeroClient myHero { get { return ObjectManager.Player; } }

        private static Vector2 circleRenderPos;

        private static Vector2 startWalkPos;
        private static float startWalkTime = 0;

<<<<<<< HEAD
        private static bool testingCollision = false;

=======
>>>>>>> origin/master
        private static float lastStopingTime = 0;

        private static IOrderedEnumerable<PositionInfo> sortedBestPos;

        private static float lastGameTimerStart = 0;
        private static float lastTickCountTimerStart = 0;
        private static float lastWatchTimerStart = 0;

        private static float lastGameTimerTick = 0;
        private static float lastTickCountTimerTick = 0;
        private static float lastWatchTimerTick = 0;

        public static float lastProcessPacketTime = 0;

        private static float getGameTimer { get { return Game.Time * 1000; } }
        private static float getTickCountTimer { get { return Environment.TickCount & int.MaxValue; } }
        private static float getWatchTimer { get { return EvadeUtils.TickCount; } }

        private static float lastTimerCheck = 0;
<<<<<<< HEAD
        private static bool lastRandomMoveCoeff = false;

        private static float lastRightMouseClickTime = 0;

        private static EvadeCommand lastTestMoveToCommand;

=======
        private static float lastRightMouseClickTime = 0;

>>>>>>> origin/master
        private static float lastSpellCastTimeEx = 0;
        private static float lastSpellCastTime = 0;
        private static float lastHeroSpellCastTime = 0;

        private static MissileClient testMissile = null;
        private static float testMissileStartTime = 0;
<<<<<<< HEAD
        private static float testMissileStartSpeed = 0;

=======
>>>>>>> origin/master
        public EvadeTester(Menu mainMenu)
        {
            lastGameTimerStart = getGameTimer;
            lastTickCountTimerStart = getTickCountTimer;
            lastWatchTimerStart = getWatchTimer;

            lastTimerCheck = getTickCountTimer;

            Drawing.OnDraw += Drawing_OnDraw;
            Player.OnIssueOrder += Game_OnIssueOrder;
            Game.OnUpdate += Game_OnGameUpdate;
            Chat.OnInput += Game_OnGameInput;

            Game.OnSendPacket += Game_onSendPacket;
            //Game.OnProcessPacket += Game_onRecvPacket;

            MissileClient.OnDelete += Game_OnDelete;

            MissileClient.OnCreate += SpellMissile_OnCreate;

            AIHeroClient.OnProcessSpellCast += Game_ProcessSpell;
            Spellbook.OnCastSpell += Game_OnCastSpell;
            //Obj_AI_Base.OnFloatPropertyChange += GameObject_OnFloatPropertyChange; TODO

            Obj_AI_Base.OnDamage += Game_OnDamage;
            //GameObject.OnIntegerPropertyChange += GameObject_OnIntegerPropertyChange;
            //Game.OnGameNotifyEvent += Game_OnGameNotifyEvent;

            Game.OnWndProc += Game_OnWndProc;

            AIHeroClient.OnNewPath += ObjAiHeroOnOnNewPath;

            SpellDetector.OnProcessDetectedSpells += SpellDetector_OnProcessDetectedSpells;

            menu = mainMenu;

            testMenu = mainMenu.IsSubMenu ? mainMenu.Parent.AddSubMenuEx("Test", "Test") : mainMenu.AddSubMenuEx("Test", "Test");
<<<<<<< HEAD
            testMenu.Add("TestWall", new CheckBox("TestWall", true));
            testMenu.Add("TestPath", new CheckBox("TestPath", true));
            testMenu.Add("TestTracker", new CheckBox("TestTracker", false));
            testMenu.Add("TestHeroPos", new CheckBox("TestHeroPos", true));
            testMenu.Add("DrawHeroPos", new CheckBox("DrawHeroPos", true));
            testMenu.Add("TestSpellEndTime", new CheckBox("TestSpellEndTime", true));
            testMenu.Add("ShowBuffs", new CheckBox("ShowBuffs", true));
            testMenu.Add("ShowDashInfo", new CheckBox("ShowDashInfo", true));
            testMenu.Add("ShowProcessSpell", new CheckBox("ShowProcessSpell", true));
            testMenu.Add("ShowMissileInfo", new CheckBox("ShowMissileInfo", true));
            testMenu.Add("ShowWindupTime", new CheckBox("ShowWindupTime", true));
=======
            testMenu.Add("TestWall", new CheckBox("TestWall"));
            testMenu.Add("TestPath", new CheckBox("TestPath"));
            testMenu.Add("TestTracker", new CheckBox("TestTracker", false));
            testMenu.Add("TestHeroPos", new CheckBox("TestHeroPos"));
            testMenu.Add("DrawHeroPos", new CheckBox("DrawHeroPos"));
            testMenu.Add("TestSpellEndTime", new CheckBox("TestSpellEndTime"));
            testMenu.Add("ShowBuffs", new CheckBox("ShowBuffs"));
            testMenu.Add("ShowDashInfo", new CheckBox("ShowDashInfo"));
            testMenu.Add("ShowProcessSpell", new CheckBox("ShowProcessSpell"));
            testMenu.Add("ShowMissileInfo", new CheckBox("ShowMissileInfo"));
            testMenu.Add("ShowWindupTime", new CheckBox("ShowWindupTime"));
>>>>>>> origin/master
            testMenu.Add("TestMoveTo", new KeyBind("TestMoveTo", false, KeyBind.BindTypes.PressToggle, 'L'));
            testMenu.Add("EvadeTesterPing", new CheckBox("EvadeTesterPing", false));

            Game_OnGameLoad();
        }

        private void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == (uint) WindowMessages.RightButtonDown)
            {
                lastRightMouseClickTime = EvadeUtils.TickCount;
            }
        }

        private void Game_onRecvPacket(GamePacketEventArgs args)
        {
           // if (args.Header == 178)
            {
                /*
                //ConsolePrinter.Print(args.GetPacketId());

                foreach (var data in args.PacketData)
                {
                    Console.Write(" " + data);
                }
                ConsolePrinter.Print("");*/

                lastProcessPacketTime = EvadeUtils.TickCount;
            }
        }

        private void Game_onSendPacket(GamePacketEventArgs args)
        {
        //    if (args.HashAlgorithm == 160)
            {
                if (testMenu.Get<CheckBox>("EvadeTesterPing").CurrentValue)
                {
                    ConsolePrinter.Print("Send Path ClickTime: " + (EvadeUtils.TickCount - lastRightMouseClickTime));
                }
            }
        }

        private void Game_OnGameLoad()
        {
            ConsolePrinter.Print("EvadeTester loaded");
            //menu.AddSubMenuEx("Test", "Test");

            //ConsolePrinter.Print("Ping:" + ObjectCache.gamePing);
            if (testMenu.Get<CheckBox>("ShowBuffs").CurrentValue)
            {
                //ConsolePrinter.Print(myHero);
            }
        }

        private void Game_OnGameInput(ChatInputEventArgs args)
        {
            ConsolePrinter.Print("" + args.Input);

        }

        private static void ObjAiHeroOnOnNewPath(Obj_AI_Base unit, GameObjectNewPathEventArgs args)
        {
            if (unit.Type == GameObjectType.AIHeroClient)
            {
                if (testMenu.Get<CheckBox>("TestSpellEndTime").CurrentValue)
                {
                    //ConsolePrinter.Print("Dash windup: " + (EvadeUtils.TickCount - EvadeSpell.lastSpellEvadeCommand.timestamp));
                }


                if (args.IsDash && testMenu.Get<CheckBox>("ShowDashInfo").CurrentValue)
                {
<<<<<<< HEAD
                    var dist = args.Path.First().Distance(args.Path.Last());
=======
                    var dist = args.Path.First().LSDistance(args.Path.Last());
>>>>>>> origin/master
                    ConsolePrinter.Print("Dash Speed: " + args.Speed + " Dash dist: " + dist);
                }

                if (unit.IsMe && testMenu.Get<CheckBox>("EvadeTesterPing").CurrentValue
                    && args.Path.Count() > 1)
                {
                    //ConsolePrinter.Print("Received Path ClickTime: " + (EvadeUtils.TickCount - lastRightMouseClickTime));
                }

                if (unit.IsMe)
                {
                    //Draw.RenderObjects.Add(new Draw.RenderCircle(args.Path.Last().To2D(), 500));
                    //Draw.RenderObjects.Add(new Draw.RenderCircle(args.Path.First().To2D(), 500));
                }

            }
        }

        private void Game_OnCastSpell(Spellbook spellbook, SpellbookCastSpellEventArgs args)
        {
            if (!spellbook.Owner.IsMe)
                return;

            if (testMenu.Get<CheckBox>("TestPath").CurrentValue)
            {
                Draw.RenderObjects.Add(new Draw.RenderCircle(args.EndPosition.To2D(), 500));
            }

            lastSpellCastTimeEx = EvadeUtils.TickCount;
        }

        private void SpellDetector_OnProcessDetectedSpells()
        {
            //var pos1 = newSpell.startPos;//SpellDetector.GetCurrentSpellPosition(newSpell);
            //DelayAction.Add(250, () => CompareSpellLocation2(newSpell));

            sortedBestPos = EvadeHelper.GetBestPositionTest();
            circleRenderPos = Evade.lastPosInfo.position;

            lastSpellCastTime = EvadeUtils.TickCount;
        }

        private void Game_OnDelete(GameObject sender, EventArgs args)
        {

            if (testMenu.Get<CheckBox>("ShowMissileInfo").CurrentValue)
            {
                if (testMissile != null && testMissile.NetworkId == sender.NetworkId)
                {
<<<<<<< HEAD
                    var range = sender.Position.To2D().Distance(testMissile.StartPosition.To2D());
=======
                    var range = sender.Position.To2D().LSDistance(testMissile.StartPosition.To2D());
>>>>>>> origin/master
                    ConsolePrinter.Print("Est.Missile range: " + range);

                    ConsolePrinter.Print("Est.Missile speed: " + range / (EvadeUtils.TickCount - testMissileStartTime));
                }
            }
        }

        private void SpellMissile_OnCreate(GameObject obj, EventArgs args)
        {
            /*if (sender.Name.ToLower().Contains("minion")
                || sender.Name.ToLower().Contains("turret")
                || sender.Type == GameObjectType.obj_GeneralParticleEmitter)
            {
                return;
            }

            if (sender.IsValid<MissileClient>())
            {
                var tMissile = sender as MissileClient;
                if (tMissile.SpellCaster.Type != GameObjectType.AIHeroClient)
                {
                    return;
                }
            }

            ConsolePrinter.Print(sender.Type + " : " + sender.Name);*/

            //if ((obj as MissileClient) != null)
            //{
            //    MissileClient autoattack = (MissileClient)obj;

            //    /*if (!autoattack.SpellCaster.IsMinion)
            //    {
            //        ConsolePrinter.Print("Missile Name " + autoattack.SData.Name);
            //        ConsolePrinter.Print("Missile Speed " + autoattack.SData.MissileSpeed);
            //        ConsolePrinter.Print("LineWidth " + autoattack.SData.LineWidth);
            //        ConsolePrinter.Print("Range " + autoattack.SData.CastRange);
            //        ConsolePrinter.Print("Accel " + autoattack.SData.MissileAccel);
            //    }*/
            //}


            //ConsolePrinter.Print(obj.Name + ": " + obj.Type);

            if (obj.GetType() != typeof(MissileClient))
                return;

            if (testMenu.Get<CheckBox>("ShowMissileInfo").CurrentValue)
            {
                return;
            }


            MissileClient missile = (MissileClient)obj;

            if (missile.SpellCaster.GetType() != typeof(AIHeroClient) || ((AIHeroClient)missile.SpellCaster).IsValid())
            {
                return;
            }


            var testMissileSpeedStartTime = EvadeUtils.TickCount;
            var testMissileSpeedStartPos = missile.Position.To2D();

            DelayAction.Add(250, () =>
            {
                if (missile != null && missile.IsValid && !missile.IsDead)
                {
                    testMissileSpeedStartTime = EvadeUtils.TickCount;
                    testMissileSpeedStartPos = missile.Position.To2D();
                }
            });

            testMissile = missile;
            testMissileStartTime = EvadeUtils.TickCount;

            ConsolePrinter.Print("Est.CastTime: " + (EvadeUtils.TickCount - lastHeroSpellCastTime));
            ConsolePrinter.Print("Missile Name " + missile.SData.Name);
            ConsolePrinter.Print("Missile Speed " + missile.SData.MissileSpeed);
            ConsolePrinter.Print("Max Speed " + missile.SData.MissileMaxSpeed);
            ConsolePrinter.Print("LineWidth " + missile.SData.LineWidth);
            ConsolePrinter.Print("Range " + missile.SData.CastRange);
            //ConsolePrinter.Print("Angle " + missile.SData.CastConeAngle);
            /*ConsolePrinter.Print("Offset: " + missile.SData.ParticleStartOffset);
            ConsolePrinter.Print("Missile Speed " + missile.SData.MissileSpeed);
            ConsolePrinter.Print("LineWidth " + missile.SData.LineWidth);
            circleRenderPos = missile.SData.ParticleStartOffset.To2D();*/

            //ConsolePrinter.Print("Acquired: " + (EvadeUtils.TickCount - lastSpellCastTime));

            Draw.RenderObjects.Add(
                new Draw.RenderCircle(missile.StartPosition.To2D(), 500));
            Draw.RenderObjects.Add(
                new Draw.RenderCircle(missile.EndPosition.To2D(), 500));

            DelayAction.Add(750, () =>
            {
                if (missile != null && missile.IsValid && !missile.IsDead)
                {
<<<<<<< HEAD
                    var dist = missile.Position.To2D().Distance(testMissileSpeedStartPos);
=======
                    var dist = missile.Position.To2D().LSDistance(testMissileSpeedStartPos);
>>>>>>> origin/master
                    ConsolePrinter.Print("Est.Missile speed: " + dist / (EvadeUtils.TickCount - testMissileSpeedStartTime));
                }
            });

            SpellData spellData;

            if (missile.SpellCaster != null && missile.SpellCaster.Team != myHero.Team &&
                missile.SData.Name != null && SpellDetector.onMissileSpells.TryGetValue(missile.SData.Name, out spellData)
                && missile.StartPosition != null && missile.EndPosition != null)
            {

<<<<<<< HEAD
                if (missile.StartPosition.Distance(myHero.Position) < spellData.range + 1000)
=======
                if (missile.StartPosition.LSDistance(myHero.Position) < spellData.range + 1000)
>>>>>>> origin/master
                {
                    var hero = missile.SpellCaster;

                    if (hero.IsVisible)
                    {
                        foreach (KeyValuePair<int, Spell> entry in SpellDetector.spells)
                        {
                            Spell spell = entry.Value;

                            if (spell.info.missileName == missile.SData.Name
                                && spell.heroID == missile.SpellCaster.NetworkId)
                            {
                                if (spell.info.isThreeWay == false && spell.info.isSpecial == false)
                                {
                                    //spell.spellObject = obj;
                                    ConsolePrinter.Print("Acquired: " + (EvadeUtils.TickCount - spell.startTime));
                                }
                            }
                        }
                    }

                }
            }
        }

        private void Game_ProcessSpell(Obj_AI_Base hero, GameObjectProcessSpellCastEventArgs args)
        {
            if (hero.IsMinion)
                return;

            if (testMenu.Get<CheckBox>("ShowProcessSpell").CurrentValue)
            {
                ConsolePrinter.Print(args.SData.Name + " CastTime: " + (hero.Spellbook.CastTime - Game.Time));

                ConsolePrinter.Print("CastRadius: " + args.SData.CastRadius);

                /*foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(args.SData))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(args.SData);
                    ConsolePrinter.Print("{0}={1}", name, value);
                }*/
            }

            if (args.SData.Name == "YasuoQW")
            {

                Draw.RenderObjects.Add(
                    new Draw.RenderCircle(args.Start.To2D(), 500));
                Draw.RenderObjects.Add(
                    new Draw.RenderCircle(args.End.To2D(), 500));
            }

            //ConsolePrinter.Print(EvadeUtils.TickCount - lastProcessPacketTime);
            //circleRenderPos = args.SData.ParticleStartOffset.To2D();

            /*Draw.RenderObjects.Add(
                new Draw.RenderPosition(args.Start.To2D(), Evade.GetTickCount + 500));
            Draw.RenderObjects.Add(
                new Draw.RenderPosition(args.End.To2D(), Evade.GetTickCount + 500));*/

            /*float testTime;
            
            
            testTime = Evade.GetTickCount;
            for (int i = 0; i < 100000; i++)
            {
                var testVar = ObjectCache.myHeroCache.boundingRadius;
            }
            ConsolePrinter.Print("Test time1: " + (Evade.GetTickCount - testTime));

            testTime = Evade.GetTickCount;
            var cacheVar = ObjectCache.myHeroCache.boundingRadius;
            for (int i = 0; i < 100000; i++)
            {
                var testVar = cacheVar;
            }
            ConsolePrinter.Print("Test time1: " + (Evade.GetTickCount - testTime));*/

            lastHeroSpellCastTime = EvadeUtils.TickCount;

            foreach (KeyValuePair<int, Spell> entry in SpellDetector.spells)
            {
                Spell spell = entry.Value;

                if (spell.info.spellName == args.SData.Name
                    && spell.heroID == hero.NetworkId)
                {
                    if (spell.info.isThreeWay == false && spell.info.isSpecial == false)
                    {
                        ConsolePrinter.Print("Time diff: " + (EvadeUtils.TickCount - spell.startTime));
                    }
                }
            }

            if (hero.IsMe)
            {
                lastSpellCastTime = EvadeUtils.TickCount;
            }
        }

        private void CompareSpellLocation(Spell spell, Vector2 pos, float time)
        {
            var pos2 = spell.currentSpellPosition;
            if (spell.spellObject != null)
            {
<<<<<<< HEAD
                ConsolePrinter.Print("Compare: " + (pos2.Distance(pos)) / (EvadeUtils.TickCount - time));
=======
                ConsolePrinter.Print("Compare: " + (pos2.LSDistance(pos)) / (EvadeUtils.TickCount - time));
>>>>>>> origin/master
            }

        }

        private void CompareSpellLocation2(Spell spell)
        {
            var pos1 = spell.currentSpellPosition;
            var timeNow = EvadeUtils.TickCount;

            if (spell.spellObject != null)
            {
<<<<<<< HEAD
                ConsolePrinter.Print("start distance: " + (spell.startPos.Distance(pos1)));
=======
                ConsolePrinter.Print("start distance: " + (spell.startPos.LSDistance(pos1)));
>>>>>>> origin/master
            }

            DelayAction.Add(250, () => CompareSpellLocation(spell, pos1, timeNow));
        }

        private void Game_OnGameUpdate(EventArgs args)
        {
            if (startWalkTime > 0)
            {
                if (EvadeUtils.TickCount - startWalkTime > 500 && myHero.IsMoving == false)
                {
<<<<<<< HEAD
                    //ConsolePrinter.Print("walkspeed: " + startWalkPos.Distance(ObjectCache.myHeroCache.serverPos2D) / (Evade.GetTickCount - startWalkTime));
=======
                    //ConsolePrinter.Print("walkspeed: " + startWalkPos.LSDistance(ObjectCache.myHeroCache.serverPos2D) / (Evade.GetTickCount - startWalkTime));
>>>>>>> origin/master
                    startWalkTime = 0;
                }
            }

            if (testMenu.Get<CheckBox>("ShowWindupTime").CurrentValue)
            {
                if (myHero.IsMoving && lastStopingTime > 0)
                {
                    ConsolePrinter.Print("WindupTime: " + (EvadeUtils.TickCount - lastStopingTime));
                    lastStopingTime = 0;
                }
                else if (!myHero.IsMoving && lastStopingTime == 0)
                {
                    lastStopingTime = EvadeUtils.TickCount;
                }
            }


            if (testMenu.Get<CheckBox>("ShowDashInfo").CurrentValue)
            {
                if (myHero.IsDashing())
                {
<<<<<<< HEAD
                    //ConsolePrinter.Print("Dash Speed: " + dashInfo.Speed + " Dash dist: " + dashInfo.EndPos.Distance(dashInfo.StartPos));
=======
                    //ConsolePrinter.Print("Dash Speed: " + dashInfo.Speed + " Dash dist: " + dashInfo.EndPos.LSDistance(dashInfo.StartPos));
>>>>>>> origin/master
                }
            }

        }

        private void Game_OnGameNotifyEvent(GameNotifyEventArgs args)
        {
            //ConsolePrinter.Print("" + args.EventId);
        }

        //private void GameObject_OnFloatPropertyChange(GameObject obj, GameObjectFloatPropertyChangeEventArgs args)
        //{
        //    //ConsolePrinter.Print(obj.Name);

        //    /*foreach (var sth in ObjectManager.Get<Obj_AI_Base>())
        //    {
        //        ConsolePrinter.Print(sth.Name);

        //    }*/

        //    if (testMenu.Item("TestSpellEndTime").Cast<CheckBox>().CurrentValue == false)
        //    {
        //        return;
        //    }

        //    if (obj.Name == "RobotBuddy")
        //    {
        //        //Draw.RenderObjects.Add(new Draw.RenderPosition(obj.Position.To2D(), EvadeUtils.TickCount + 10));
        //    }

        //    //ConsolePrinter.Print(obj.Name);


        //    if (args.Property == "mHP" && args.OldValue > args.NewValue)
        //    {
        //        //ConsolePrinter.Print("Damage taken time: " + (EvadeUtils.TickCount - lastSpellCastTime));
        //    }

        //    if (!obj.IsMe)
        //    {
        //        return;
        //    }



        //    if (args.Property != "mExp" && args.Property != "mGold" && args.Property != "mGoldTotal"
        //        && args.Property != "mMP" && args.Property != "mPARRegenRate")
        //    {
        //        //ConsolePrinter.Print(args.Property + ": " + args.NewValue);
        //    }
        //}

        private void Game_OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (!testMenu.Get<CheckBox>("TestSpellEndTime").CurrentValue)
            {
                return;
            }

            if (!sender.IsMe)
                return;

            ConsolePrinter.Print("Damage taken time: " + (EvadeUtils.TickCount - lastSpellCastTime));
        }

        //private void GameObject_OnIntegerPropertyChange(GameObject obj, GameObjectIntegerPropertyChangeEventArgs args)
        //{
        //    if (obj.IsMe)
        //    {
        //        if (args.Property != "mExp" && args.Property != "mGold" && args.Property != "mGoldTotal")
        //        {
        //            ConsolePrinter.Print("Int" + args.Property + ": " + args.NewValue);
        //        }

        //    }
        //}

        private void Game_OnIssueOrder(Obj_AI_Base hero, PlayerIssueOrderEventArgs args)
        {
            if (!hero.IsMe)
                return;

            if (args.Order == GameObjectOrder.HoldPosition)
            {
                var path = myHero.Path;
                var heroPoint = ObjectCache.myHeroCache.serverPos2D;


                if (path.Length > 0)
                {
                    var movePos = path[path.Length - 1].To2D();
                    var walkDir = (movePos - heroPoint).Normalized();

                    //circleRenderPos = EvadeHelper.GetRealHeroPos();
                    //heroPoint;// +walkDir * ObjectCache.myHeroCache.moveSpeed * (((float)ObjectCache.gamePing) / 1000);
                }
            }

            if (testMenu.Get<CheckBox>("TestPath").CurrentValue)
            {
                var tPath = myHero.GetPath(args.TargetPosition);
                Vector2 lastPoint = Vector2.Zero;

                foreach (Vector3 point in tPath)
                {
                    var point2D = point.To2D();
                    Draw.RenderObjects.Add(new Draw.RenderCircle(point2D, 500));
                    //Render.Circle.DrawCircle(new Vector3(point.X, point.Y, point.Z), ObjectCache.myHeroCache.boundingRadius, Color.Violet, 3);
                }
            }

            /*
            if (args.Order == GameObjectOrder.MoveTo)
            {         
                if (testingCollision)
                {
<<<<<<< HEAD
                    if (args.TargetPosition.To2D().Distance(testCollisionPos) < 3)
=======
                    if (args.TargetPosition.To2D().LSDistance(testCollisionPos) < 3)
>>>>>>> origin/master
                    {
                        //var path = myHero.GetPath();
                        //circleRenderPos

                        args.Process = false;
                    }
                }
            }*/

            if (args.Order == GameObjectOrder.MoveTo)
            {
                if (testMenu.Get<CheckBox>("EvadeTesterPing").CurrentValue)
                {
                    ConsolePrinter.Print("Sending Path ClickTime: " + (EvadeUtils.TickCount - lastRightMouseClickTime));
                }

                Vector2 heroPos = ObjectCache.myHeroCache.serverPos2D;
                Vector2 pos = args.TargetPosition.To2D();
                float speed = ObjectCache.myHeroCache.moveSpeed;

                startWalkPos = heroPos;
                startWalkTime = EvadeUtils.TickCount;

                foreach (KeyValuePair<int, Spell> entry in SpellDetector.spells)
                {
                    Spell spell = entry.Value;
                    var spellPos = spell.currentSpellPosition;
                    var walkDir = (pos - heroPos).Normalized();


                    float spellTime = (EvadeUtils.TickCount - spell.startTime) - spell.info.spellDelay;
                    spellPos = spell.startPos + spell.direction * spell.info.projectileSpeed * (spellTime / 1000);
                    //ConsolePrinter.Print("aaaa" + spellTime);


                    bool isCollision = false;
                    float movingCollisionTime = MathUtils.GetCollisionTime(heroPos, spellPos, walkDir * (speed - 25), spell.direction * (spell.info.projectileSpeed - 200), ObjectCache.myHeroCache.boundingRadius, spell.radius, out isCollision);
                    if (isCollision)
                    {
<<<<<<< HEAD
                        //ConsolePrinter.Print("aaaa" + spellPos.Distance(spell.endPos) / spell.info.projectileSpeed);
                        if (true)//spellPos.Distance(spell.endPos) / spell.info.projectileSpeed > movingCollisionTime)
=======
                        //ConsolePrinter.Print("aaaa" + spellPos.LSDistance(spell.endPos) / spell.info.projectileSpeed);
                        if (true)//spellPos.LSDistance(spell.endPos) / spell.info.projectileSpeed > movingCollisionTime)
>>>>>>> origin/master
                        {
                            ConsolePrinter.Print("movingCollisionTime: " + movingCollisionTime);
                            //circleRenderPos = heroPos + walkDir * speed * movingCollisionTime;
                        }

                    }
                }
            }
        }

        private void GetPath(Vector2 movePos)
        {

        }

        private void PrintTimers()
        {
            Drawing.DrawText(10, 10, Color.White, "Timer1 Freq: " + (getGameTimer - lastGameTimerTick));
            Drawing.DrawText(10, 30, Color.White, "Timer2 Freq: " + (getTickCountTimer - lastTickCountTimerTick));
            Drawing.DrawText(10, 50, Color.White, "Timer3 Freq: " + (getWatchTimer - lastWatchTimerTick));//(getWatchTimer - lastWatchTimerTick));

            if (getTickCountTimer - lastTimerCheck > 1000)
            {
                ConsolePrinter.Print("" + ((getGameTimer - lastGameTimerStart) - (getTickCountTimer - lastTickCountTimerStart)));
                lastTimerCheck = getTickCountTimer;
            }


            Drawing.DrawText(10, 70, Color.White, "Timer1 Freq: " + (getGameTimer - lastGameTimerStart));
            Drawing.DrawText(10, 90, Color.White, "Timer2 Freq: " + (getTickCountTimer - lastTickCountTimerStart));
            Drawing.DrawText(10, 110, Color.White, "Timer3 Freq: " + (getWatchTimer - lastWatchTimerStart));

            /*Drawing.DrawText(10, 70, Color.White, "Timer1 Freq: " + (getGameTimer));
            Drawing.DrawText(10, 90, Color.White, "Timer2 Freq: " + (getTickCountTimer));
            Drawing.DrawText(10, 100, Color.White, "Timer3 Freq: " + (getWatchTimer));*/



            lastGameTimerTick = getGameTimer;
            lastTickCountTimerTick = getTickCountTimer;
            lastWatchTimerTick = getWatchTimer;
        }

        //private void TestUnderTurret()
        //{
        //    if (Game.CursorPos.To2D().IsUnderTurret())
        //    {
        //        Render.Circle.DrawCircle(Game.CursorPos, 50, Color.Red, 3);
        //    }
        //    else
        //    {
        //        Render.Circle.DrawCircle(Game.CursorPos, 50, Color.White, 3);
        //    }
        //}

        private void Drawing_OnDraw(EventArgs args)
        {
            //PrintTimers();

            //EvadeHelper.CheckMovePath(Game.CursorPos.To2D());            

            //TestUnderTurret();


            /*if (EvadeHelper.CheckPathCollision(myHero, Game.CursorPos.To2D()))
            {                
                var paths = myHero.GetPath(ObjectCache.myHeroCache.serverPos2DExtra.To3D(), Game.CursorPos);
                foreach (var path in paths)
                {
                    Render.Circle.DrawCircle(path, ObjectCache.myHeroCache.boundingRadius, Color.Red, 3);
                }
            }
            else
            {
                Render.Circle.DrawCircle(Game.CursorPos, ObjectCache.myHeroCache.boundingRadius, Color.White, 3);
            }*/

            foreach (KeyValuePair<int, Spell> entry in SpellDetector.drawSpells)
            {
                Spell spell = entry.Value;

                if (spell.spellType == SpellType.Line)
                {
                    Vector2 spellPos = spell.currentSpellPosition;


<<<<<<< HEAD
                    Render.Circle.DrawCircle(new Vector3(spellPos.X, spellPos.Y, myHero.Position.Z), spell.info.radius, Color.White, 3);
=======
                    LeagueSharp.Common.Render.Circle.DrawCircle(new Vector3(spellPos.X, spellPos.Y, myHero.Position.Z), spell.info.radius, Color.White, 3);
>>>>>>> origin/master

                    /*spellPos = spellPos + spell.direction * spell.info.projectileSpeed * (60 / 1000); //move the spellPos by 50 miliseconds forwards
                    spellPos = spellPos + spell.direction * 200; //move the spellPos by 50 units forwards        

                    Render.Circle.DrawCircle(new Vector3(spellPos.X, spellPos.Y, myHero.Position.Z), spell.info.radius, Color.White, 3);*/
                }
            }

            if (testMenu.Get<CheckBox>("TestHeroPos").CurrentValue)
            {
                var path = myHero.Path;
                if (path.Length > 0)
                {
                    var heroPos2 = EvadeHelper.GetRealHeroPos(ObjectCache.gamePing + 50);// path[path.Length - 1].To2D();
                    var heroPos1 = ObjectCache.myHeroCache.serverPos2D;

<<<<<<< HEAD
                    Render.Circle.DrawCircle(new Vector3(heroPos2.X, heroPos2.Y, myHero.ServerPosition.Z), ObjectCache.myHeroCache.boundingRadius, Color.Red, 3);
                    Render.Circle.DrawCircle(new Vector3(myHero.ServerPosition.X, myHero.ServerPosition.Y, myHero.ServerPosition.Z), ObjectCache.myHeroCache.boundingRadius, Color.White, 3);

                    var heroPos = Drawing.WorldToScreen(ObjectManager.Player.Position);
                    var dimension = Drawing.GetTextEntent("Evade: ON", 12);
                    Drawing.DrawText(heroPos.X - dimension.Width / 2, heroPos.Y, Color.Red, "" + (int)(heroPos2.Distance(heroPos1)));

                    Render.Circle.DrawCircle(new Vector3(circleRenderPos.X, circleRenderPos.Y, myHero.ServerPosition.Z), 10, Color.Red, 3);
=======
                    LeagueSharp.Common.Render.Circle.DrawCircle(new Vector3(heroPos2.X, heroPos2.Y, myHero.ServerPosition.Z), ObjectCache.myHeroCache.boundingRadius, Color.Red, 3);
                    LeagueSharp.Common.Render.Circle.DrawCircle(new Vector3(myHero.ServerPosition.X, myHero.ServerPosition.Y, myHero.ServerPosition.Z), ObjectCache.myHeroCache.boundingRadius, Color.White, 3);

                    var heroPos = Drawing.WorldToScreen(ObjectManager.Player.Position);
                    var dimension = Drawing.GetTextEntent("Evade: ON", 12);
                    Drawing.DrawText(heroPos.X - dimension.Width / 2, heroPos.Y, Color.Red, "" + (int)(heroPos2.LSDistance(heroPos1)));

                    LeagueSharp.Common.Render.Circle.DrawCircle(new Vector3(circleRenderPos.X, circleRenderPos.Y, myHero.ServerPosition.Z), 10, Color.Red, 3);
>>>>>>> origin/master
                }
            }

            if (testMenu.Get<CheckBox>("DrawHeroPos").CurrentValue)
            {
<<<<<<< HEAD
                Render.Circle.DrawCircle(new Vector3(myHero.ServerPosition.X, myHero.ServerPosition.Y, myHero.ServerPosition.Z), ObjectCache.myHeroCache.boundingRadius, Color.White, 3);
=======
                LeagueSharp.Common.Render.Circle.DrawCircle(new Vector3(myHero.ServerPosition.X, myHero.ServerPosition.Y, myHero.ServerPosition.Z), ObjectCache.myHeroCache.boundingRadius, Color.White, 3);
>>>>>>> origin/master
            }


            if (testMenu.Get<KeyBind>("TestMoveTo").CurrentValue)
            {
                //var keyBind = testMenu.Get<KeyBind>("TestMoveTo"); TODO: ??
                //testMenu.Item("TestMoveTo").SetValue(new KeyBind(keyBind.Key, KeyBindType.Toggle, false));

                /*lastRightMouseClickTime = EvadeUtils.TickCount;
                myHero.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos,false);*/

                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

                var dir = (Game.CursorPos - myHero.Position).Normalized();
<<<<<<< HEAD
                //var pos2 = myHero.Position - dir * Game.CursorPos.Distance(myHero.Position);
=======
                //var pos2 = myHero.Position - dir * Game.CursorPos.LSDistance(myHero.Position);
>>>>>>> origin/master

                //var pos2 = myHero.Position.To2D() - dir.To2D() * 75;
                var pos2 = Game.CursorPos.To2D() - dir.To2D() * 75;

<<<<<<< HEAD
                //Console.WriteLine(myHero.BBox.Maximum.Distance(myHero.Position));
=======
                //Console.WriteLine(myHero.BBox.Maximum.LSDistance(myHero.Position));
>>>>>>> origin/master

                DelayAction.Add(20, () => Player.IssueOrder(GameObjectOrder.MoveTo, pos2.To3D(), false));
                //myHero.IssueOrder(GameObjectOrder.MoveTo, pos2, false);
            }

            if (testMenu.Get<CheckBox>("TestPath").CurrentValue)
            {
                var tPath = myHero.GetPath(Game.CursorPos);
                Vector2 lastPoint = Vector2.Zero;

                foreach (Vector3 point in tPath)
                {
                    var point2D = point.To2D();
<<<<<<< HEAD
                    Render.Circle.DrawCircle(new Vector3(point.X, point.Y, point.Z), ObjectCache.myHeroCache.boundingRadius, Color.Violet, 3);
=======
                    LeagueSharp.Common.Render.Circle.DrawCircle(new Vector3(point.X, point.Y, point.Z), ObjectCache.myHeroCache.boundingRadius, Color.Violet, 3);
>>>>>>> origin/master

                    lastPoint = point2D;
                }
            }

            if (testMenu.Get<CheckBox>("TestPath").CurrentValue)
            {
                var tPath = myHero.GetPath(Game.CursorPos);
                Vector2 lastPoint = Vector2.Zero;

                foreach (Vector3 point in tPath)
                {
                    var point2D = point.To2D();
                    //Render.Circle.DrawCircle(new Vector3(point.X, point.Y, point.Z), ObjectCache.myHeroCache.boundingRadius, Color.Violet, 3);

                    lastPoint = point2D;
                }

                foreach (KeyValuePair<int, Spell> entry in SpellDetector.spells)
                {
                    Spell spell = entry.Value;

                    Vector2 to = Game.CursorPos.To2D();
                    var dir = (to - myHero.Position.To2D()).Normalized();
<<<<<<< HEAD
                    Vector2 cPos1, cPos2;
=======
>>>>>>> origin/master

                    var cpa = MathUtilsCPA.CPAPointsEx(myHero.Position.To2D(), dir * ObjectCache.myHeroCache.moveSpeed, spell.endPos, spell.direction * spell.info.projectileSpeed, to, spell.endPos);
                    var cpaTime = MathUtilsCPA.CPATime(myHero.Position.To2D(), dir * ObjectCache.myHeroCache.moveSpeed, spell.endPos, spell.direction * spell.info.projectileSpeed);

                    //ConsolePrinter.Print("" + cpaTime);
                    //Render.Circle.DrawCircle(cPos1.To3D(), ObjectCache.myHeroCache.boundingRadius, Color.Red, 3);

                    if (cpa < ObjectCache.myHeroCache.boundingRadius + spell.radius)
                    {

                    }
                }
            }

            if (testMenu.Get<CheckBox>("ShowBuffs").CurrentValue)
            {
                var target = myHero;

                foreach (var hero in EntityManager.Heroes.Enemies)
                {
                    target = hero;
                }

                var buffs = target.Buffs;

                //ConsolePrinter.Print(myHero.ChampionName);

                //if(myHero.IsDead)
                //    ConsolePrinter.Print("dead");

                if (!target.IsTargetable)
                    ConsolePrinter.Print("invul" + EvadeUtils.TickCount);

                int height = 20;

                foreach (var buff in buffs)
                {
                    if (buff.IsValid())
                    {
                        Drawing.DrawText(10, height, Color.White, buff.Name);
                        height += 20;

                        ConsolePrinter.Print(buff.Name);
                    }
                }
            }

            if (testMenu.Get<CheckBox>("TestTracker").CurrentValue)
            {
                foreach (KeyValuePair<int, ObjectTrackerInfo> entry in ObjectTracker.objTracker)
                {
                    var info = entry.Value;

                    Vector3 endPos2;
<<<<<<< HEAD
                    if (info.usePosition == false)
                        endPos2 = info.obj.Position;
                    else
                        endPos2 = info.position;

                    Render.Circle.DrawCircle(new Vector3(endPos2.X, endPos2.Y, myHero.Position.Z), 50, Color.Green, 3);
=======
                    endPos2 = info.usePosition == false ? info.obj.Position : info.position;

                    LeagueSharp.Common.Render.Circle.DrawCircle(new Vector3(endPos2.X, endPos2.Y, myHero.Position.Z), 50, Color.Green, 3);
>>>>>>> origin/master
                }


                /*foreach (var obj in ObjectManager.Get<Obj_AI_Minion>())
                {
                    ConsolePrinter.Print("minion: " + obj.Name);
                    if (obj.Name == "Ekko")
                    {
                        var pos = obj.Position;
                        Render.Circle.DrawCircle(pos, 100, Color.Green, 3);
                    }
                }*/
            }

            if (testMenu.Get<CheckBox>("ShowMissileInfo").CurrentValue)
            {
                if (testMissile != null)
                {
                    //Render.Circle.DrawCircle(testMissile.Position, testMissile.BoundingRadius, Color.White, 3);
                    
                }
            }

            if (testMenu.Get<CheckBox>("TestWall").CurrentValue)
            {
                /*foreach (var posInfo in sortedBestPos)
                {
                    var posOnScreen = Drawing.WorldToScreen(posInfo.position.To3D());
                    //Drawing.DrawText(posOnScreen.X, posOnScreen.Y, Color.Aqua, "" + (int)posInfo.closestDistance);

                    
                    if (!posInfo.rejectPosition)
                    {
                        Drawing.DrawText(posOnScreen.X, posOnScreen.Y, Color.Aqua, "" + (int)posInfo.closestDistance);
                    }

                    Drawing.DrawText(posOnScreen.X, posOnScreen.Y, Color.Aqua, "" + (int)posInfo.closestDistance);

                    if (posInfo.posDangerCount <= 0)
                    {
                        var pos = posInfo.position;
                        Render.Circle.DrawCircle(new Vector3(pos.X, pos.Y, myHero.Position.Z), (float)25, Color.White, 3);
                    }                                      
                }*/

                int posChecked = 0;
                int maxPosToCheck = 50;
                int posRadius = 50;
                int radiusIndex = 0;

                Vector2 heroPoint = ObjectCache.myHeroCache.serverPos2D;
                List<PositionInfo> posTable = new List<PositionInfo>();

                while (posChecked < maxPosToCheck)
                {
                    radiusIndex++;

                    int curRadius = radiusIndex * (2 * posRadius);
                    int curCircleChecks = (int)Math.Ceiling((2 * Math.PI * (double)curRadius) / (2 * (double)posRadius));

                    for (int i = 1; i < curCircleChecks; i++)
                    {
                        posChecked++;
                        var cRadians = (2 * Math.PI / (curCircleChecks - 1)) * i; //check decimals
                        var pos = new Vector2((float)Math.Floor(heroPoint.X + curRadius * Math.Cos(cRadians)), (float)Math.Floor(heroPoint.Y + curRadius * Math.Sin(cRadians)));

                        if (EvadeHelper.CheckPathCollision(myHero, pos))
                        {
<<<<<<< HEAD
                            Render.Circle.DrawCircle(new Vector3(pos.X, pos.Y, myHero.Position.Z), (float)25, Color.White, 3);
=======
                            LeagueSharp.Common.Render.Circle.DrawCircle(new Vector3(pos.X, pos.Y, myHero.Position.Z), (float)25, Color.White, 3);
>>>>>>> origin/master
                        }

                    }
                }
            }

        }
    }
}
