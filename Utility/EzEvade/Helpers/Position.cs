﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
<<<<<<< HEAD
=======
using LeagueSharp.Common;
>>>>>>> origin/master

namespace ezEvade
{
    public static class Position
    {
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }

        public static int CheckPosDangerLevel(this Vector2 pos, float extraBuffer)
        {
<<<<<<< HEAD
            var dangerlevel = 0;
            foreach (KeyValuePair<int, Spell> entry in SpellDetector.spells)
            {
                Spell spell = entry.Value;

                if (pos.InSkillShot(spell, ObjectCache.myHeroCache.boundingRadius + extraBuffer))
                {
                    dangerlevel += spell.dangerlevel;
                }
            }
            return dangerlevel;
=======
            return SpellDetector.spells.Select(entry => entry.Value).Where(spell => pos.InSkillShot(spell, ObjectCache.myHeroCache.boundingRadius + extraBuffer)).Sum(spell => spell.dangerlevel);
>>>>>>> origin/master
        }

        public static bool InSkillShot(this Vector2 position, Spell spell, float radius, bool predictCollision = true)
        {
            if (spell.spellType == SpellType.Line)
            {
                Vector2 spellPos = spell.currentSpellPosition;
                Vector2 spellEndPos = predictCollision ? spell.GetSpellEndPosition() : spell.endPos;

                //spellPos = spellPos - spell.direction * radius; //leave some space at back of spell
                //spellEndPos = spellEndPos + spell.direction * radius; //leave some space at the front of spell

                /*if (spell.info.projectileSpeed == float.MaxValue
                    && Evade.GetTickCount - spell.startTime > spell.info.spellDelay)
                {
                    return false;
                }*/

                var projection = position.ProjectOn(spellPos, spellEndPos);

<<<<<<< HEAD
                /*if (projection.SegmentPoint.Distance(spellEndPos) < 100) //Check Skillshot endpoints
=======
                /*if (projection.SegmentPoint.LSDistance(spellEndPos) < 100) //Check Skillshot endpoints
>>>>>>> origin/master
                {
                    //unfinished
                }*/

<<<<<<< HEAD
                return projection.IsOnSegment && projection.SegmentPoint.Distance(position) <= spell.radius + radius;
=======
                return projection.IsOnSegment && projection.SegmentPoint.LSDistance(position) <= spell.radius + radius;
>>>>>>> origin/master
            }
            else if (spell.spellType == SpellType.Circular)
            {
                if (spell.info.spellName == "VeigarEventHorizon")
                {
<<<<<<< HEAD
                    return position.Distance(spell.endPos) <= spell.radius + radius - ObjectCache.myHeroCache.boundingRadius
                        && position.Distance(spell.endPos) >= spell.radius + radius - ObjectCache.myHeroCache.boundingRadius - 125;
                }

                return position.Distance(spell.endPos) <= spell.radius + radius - ObjectCache.myHeroCache.boundingRadius;
=======
                    return position.LSDistance(spell.endPos) <= spell.radius + radius - ObjectCache.myHeroCache.boundingRadius
                        && position.LSDistance(spell.endPos) >= spell.radius + radius - ObjectCache.myHeroCache.boundingRadius - 125;
                }

                return position.LSDistance(spell.endPos) <= spell.radius + radius - ObjectCache.myHeroCache.boundingRadius;
>>>>>>> origin/master
            }
            else if (spell.spellType == SpellType.Arc)
            {
                if (position.isLeftOfLineSegment(spell.startPos, spell.endPos))
                {
                    return false;
                }

<<<<<<< HEAD
                var spellRange = spell.startPos.Distance(spell.endPos);
                var midPoint = spell.startPos + spell.direction * (spellRange/2);

                return position.Distance(midPoint) <= spell.radius + radius - ObjectCache.myHeroCache.boundingRadius;
=======
                var spellRange = spell.startPos.LSDistance(spell.endPos);
                var midPoint = spell.startPos + spell.direction * (spellRange/2);

                return position.LSDistance(midPoint) <= spell.radius + radius - ObjectCache.myHeroCache.boundingRadius;
>>>>>>> origin/master
            }
            else if (spell.spellType == SpellType.Cone)
            {

            }
            return false;
        }

        public static bool isLeftOfLineSegment(this Vector2 pos, Vector2 start, Vector2 end)
        {
            return ((end.X - start.X) * (pos.Y - start.Y) - (end.Y - start.Y) * (pos.X - start.X)) > 0;
        }

        public static float GetDistanceToTurrets(this Vector2 pos)
        {
            float minDist = float.MaxValue;

            foreach (var entry in ObjectCache.turrets)
            {
                var turret = entry.Value;
                if (turret == null || !turret.IsValid || turret.IsDead)
                {
                    Core.DelayAction(() => ObjectCache.turrets.Remove(entry.Key), 1);
                    continue;
                }

                if (turret.IsAlly)
                {
                    continue;
                }

<<<<<<< HEAD
                var distToTurret = pos.Distance(turret.Position.To2D());
=======
                var distToTurret = pos.LSDistance(turret.Position.To2D());
>>>>>>> origin/master

                minDist = Math.Min(minDist, distToTurret);
            }

            return minDist;
        }

        public static float GetDistanceToChampions(this Vector2 pos)
        {
<<<<<<< HEAD
            float minDist = float.MaxValue;

            foreach (var hero in EntityManager.Heroes.Enemies)
            {
                if (hero != null && hero.IsValid && !hero.IsDead && hero.IsVisible)
                {
                    var heroPos = hero.ServerPosition.To2D();
                    var dist = heroPos.Distance(pos);

                    minDist = Math.Min(minDist, dist);
                }
            }

            return minDist;
=======
            return (from hero in EntityManager.Heroes.Enemies where hero != null && hero.IsValid && !hero.IsDead && hero.IsVisible select hero.ServerPosition.To2D() into heroPos select heroPos.LSDistance(pos)).Concat(new[] {float.MaxValue}).Min();
>>>>>>> origin/master
        }

        public static bool HasExtraAvoidDistance(this Vector2 pos, float extraBuffer)
        {
<<<<<<< HEAD
            foreach (KeyValuePair<int, Spell> entry in SpellDetector.spells)
            {
                Spell spell = entry.Value;

                if (spell.spellType == SpellType.Line)
                {
                    if (pos.InSkillShot(spell, ObjectCache.myHeroCache.boundingRadius + extraBuffer))
                    {
                        return true;
                    }
                }
            }
            return false;
=======
            return SpellDetector.spells.Select(entry => entry.Value).Where(spell => spell.spellType == SpellType.Line).Any(spell => pos.InSkillShot(spell, ObjectCache.myHeroCache.boundingRadius + extraBuffer));
>>>>>>> origin/master
        }

        public static float GetPositionValue(this Vector2 pos)
        {
<<<<<<< HEAD
            float posValue = pos.Distance(Game.CursorPos.To2D());
=======
            float posValue = pos.LSDistance(Game.CursorPos.To2D());
>>>>>>> origin/master

            if (ObjectCache.menuCache.cache["PreventDodgingNearEnemy"].Cast<CheckBox>().CurrentValue)
            {
                var minComfortDistance = ObjectCache.menuCache.cache["MinComfortZone"].Cast<Slider>().CurrentValue;
                var distanceToChampions = pos.GetDistanceToChampions();

                if (minComfortDistance > distanceToChampions)
                {
                    posValue += 2 * (minComfortDistance - distanceToChampions);
                }
            }

            if (ObjectCache.menuCache.cache["PreventDodgingUnderTower"].Cast<CheckBox>().CurrentValue)
            {
                var turretRange = 875 + ObjectCache.myHeroCache.boundingRadius;
                var distanceToTurrets = pos.GetDistanceToTurrets();

                if (turretRange > distanceToTurrets)
                {
                    posValue += 5 * (turretRange - distanceToTurrets);
                }
            }

            return posValue;
        }

        public static bool CheckDangerousPos(this Vector2 pos, float extraBuffer, bool checkOnlyDangerous = false)
        {
<<<<<<< HEAD
            foreach (KeyValuePair<int, Spell> entry in SpellDetector.spells)
            {
                Spell spell = entry.Value;

                if (checkOnlyDangerous && spell.dangerlevel < 3)
                {
                    continue;
                }

                if (pos.InSkillShot(spell, ObjectCache.myHeroCache.boundingRadius + extraBuffer))
                {
                    return true;
                }
            }
            return false;
=======
            return SpellDetector.spells.Select(entry => entry.Value).Where(spell => !checkOnlyDangerous || spell.dangerlevel >= 3).Any(spell => pos.InSkillShot(spell, ObjectCache.myHeroCache.boundingRadius + extraBuffer));
>>>>>>> origin/master
        }

        public static List<Vector2> GetSurroundingPositions(int maxPosToCheck = 150, int posRadius = 25)
        {
            List<Vector2> positions = new List<Vector2>();

            int posChecked = 0;
            int radiusIndex = 0;

            Vector2 heroPoint = ObjectCache.myHeroCache.serverPos2D;
            Vector2 lastMovePos = Game.CursorPos.To2D();

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
                    var pos = new Vector2((float)Math.Floor(heroPoint.X + curRadius * Math.Cos(cRadians)),
                                          (float)Math.Floor(heroPoint.Y + curRadius * Math.Sin(cRadians)));

                    positions.Add(pos);
                }
            }

            return positions;
        }
    }
}
