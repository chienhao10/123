using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class Spell
    {
        public float startTime;
        public float endTime;
        public Vector2 startPos;
        public Vector2 endPos;
        public Vector2 direction;
        public float height;
        public int heroID;
        public int projectileID;
        public SpellData info;
        public int spellID;
        public GameObject spellObject = null;
        public SpellType spellType;

        public Vector2 currentSpellPosition = Vector2.Zero;
        public Vector2 currentNegativePosition = Vector2.Zero;
        public Vector2 predictedEndPos = Vector2.Zero;

        public float radius = 0;
        public int dangerlevel = 1;

        public float evadeTime = float.MinValue;
        public float spellHitTime = float.MinValue;

        public Spell()
        {

        }
    }

    public static class SpellExtensions
    {
        public static float GetSpellRadius(this Spell spell)
        {
            var radius = ObjectCache.menuCache.cache[spell.info.spellName + "SpellRadius"].Cast<Slider>().CurrentValue;
            var extraRadius = ObjectCache.menuCache.cache["ExtraSpellRadius"].Cast<Slider>().CurrentValue;

            if (spell.info.hasEndExplosion && spell.spellType == SpellType.Circular)
            {
                return spell.info.secondaryRadius + extraRadius;
            }

            if (spell.spellType == SpellType.Arc)
            {
<<<<<<< HEAD
                var spellRange = spell.startPos.Distance(spell.endPos);
                var arcRadius = spell.info.radius * (1 + spellRange / 100) + extraRadius;

=======
                var spellRange = spell.startPos.LSDistance(spell.endPos);
                var arcRadius = spell.info.radius * (1 + spellRange/100) + extraRadius;
                                
>>>>>>> origin/master
                return arcRadius;
            }

            return (float)(radius + extraRadius);
        }

        public static int GetSpellDangerLevel(this Spell spell)
        {
            var dangerStr = ObjectCache.menuCache.cache[spell.info.spellName + "DangerLevel"].Cast<Slider>().DisplayName;

            var dangerlevel = 1;

            switch (dangerStr)
            {
                case "Low":
                    dangerlevel = 1;
                    break;
                case "High":
                    dangerlevel = 3;
                    break;
                case "Extreme":
                    dangerlevel = 4;
                    break;
                default:
                    dangerlevel = 2;
                    break;
            }

            return dangerlevel;
        }

        public static string GetSpellDangerString(this Spell spell)
        {
            switch (spell.GetSpellDangerLevel())
            {
                case 1:
                    return "Low";
                case 3:
                    return "High";
                case 4:
                    return "Extreme";
                default:
                    return "Normal";
            }
        }

        public static bool hasProjectile(this Spell spell)
        {
            return spell.info.projectileSpeed > 0 && spell.info.projectileSpeed != float.MaxValue;
        }

        public static Vector2 GetSpellProjection(this Spell spell, Vector2 pos, bool predictPos = false)
        {
            if (spell.spellType == SpellType.Line
                || spell.spellType == SpellType.Arc)
            {
                if (predictPos)
                {
                    var spellPos = spell.currentSpellPosition;
                    var spellEndPos = spell.GetSpellEndPosition();

                    return pos.ProjectOn(spellPos, spellEndPos).SegmentPoint;
                }
                else
                {
                    return pos.ProjectOn(spell.startPos, spell.endPos).SegmentPoint;
                }
            }
            else if (spell.spellType == SpellType.Circular)
            {
                return spell.endPos;
            }

            return Vector2.Zero;
        }

        public static Obj_AI_Base CheckSpellCollision(this Spell spell)
        {
            if (spell.info.collisionObjects.Count() < 1)
            {
                return null;
            }

            List<Obj_AI_Base> collisionCandidates = new List<Obj_AI_Base>();
            var spellPos = spell.currentSpellPosition;
<<<<<<< HEAD
            var distanceToHero = spellPos.Distance(ObjectCache.myHeroCache.serverPos2D);

            if (spell.info.collisionObjects.Contains(CollisionObjectType.EnemyChampions))
            {
                foreach (var hero in EntityManager.Heroes.Allies
                    .Where(h => !h.IsMe && h.IsValidTarget(distanceToHero, false, spellPos.To3D())))
                {
                    collisionCandidates.Add(hero);
                }
=======
            var distanceToHero = spellPos.LSDistance(ObjectCache.myHeroCache.serverPos2D);

            if (spell.info.collisionObjects.Contains(CollisionObjectType.EnemyChampions))
            {
                collisionCandidates.AddRange(EntityManager.Heroes.Allies.Where(h => !h.IsMe && h.IsValidTarget(distanceToHero)).Cast<Obj_AI_Base>());
>>>>>>> origin/master
            }

            if (spell.info.collisionObjects.Contains(CollisionObjectType.EnemyMinions))
            {
<<<<<<< HEAD
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>()
                    .Where(h => h.Team == Evade.myHero.Team && h.IsValidTarget(distanceToHero, false, spellPos.To3D())))
                {
                    if (minion.CharData.BaseSkinName.ToLower() == "teemomushroom"
                        || minion.CharData.BaseSkinName.ToLower() == "shacobox")
                    {
                        continue;
                    }

                    collisionCandidates.Add(minion);
                }
            }

            var sortedCandidates = collisionCandidates.OrderBy(h => h.Distance(spellPos));

            foreach (var candidate in sortedCandidates)
            {
                if (candidate.ServerPosition.To2D().InSkillShot(spell, candidate.BoundingRadius, false))
                {
                    return candidate;
                }
            }

            return null;
=======
                collisionCandidates.AddRange(ObjectManager.Get<Obj_AI_Minion>().Where(h => h.Team == Evade.myHero.Team && h.IsValidTarget()).Where(minion => minion.CharData.BaseSkinName.ToLower() != "teemomushroom" && minion.CharData.BaseSkinName.ToLower() != "shacobox").Cast<Obj_AI_Base>());
            }

            var sortedCandidates = collisionCandidates.OrderBy(h => h.LSDistance(spellPos));

            return sortedCandidates.FirstOrDefault(candidate => candidate.ServerPosition.To2D().InSkillShot(spell, candidate.BoundingRadius, false));
>>>>>>> origin/master
        }

        public static float GetSpellHitTime(this Spell spell, Vector2 pos)
        {

            if (spell.spellType == SpellType.Line)
            {
                if (spell.info.projectileSpeed == float.MaxValue)
                {
                    return Math.Max(0, spell.endTime - EvadeUtils.TickCount - ObjectCache.gamePing);
                }

                var spellPos = spell.GetCurrentSpellPosition(true, ObjectCache.gamePing);
<<<<<<< HEAD
                return 1000 * spellPos.Distance(pos) / spell.info.projectileSpeed;
=======
                return 1000 * spellPos.LSDistance(pos) / spell.info.projectileSpeed;
>>>>>>> origin/master
            }
            else if (spell.spellType == SpellType.Circular)
            {
                return Math.Max(0, spell.endTime - EvadeUtils.TickCount - ObjectCache.gamePing);
            }

            return float.MaxValue;
        }

        public static bool CanHeroEvade(this Spell spell, Obj_AI_Base hero, out float rEvadeTime, out float rSpellHitTime)
        {
            var heroPos = hero.ServerPosition.To2D();
            float evadeTime = 0;
            float spellHitTime = 0;

            if (spell.spellType == SpellType.Line)
            {
                var projection = heroPos.ProjectOn(spell.startPos, spell.endPos).SegmentPoint;
<<<<<<< HEAD
                evadeTime = 1000 * (spell.radius - heroPos.Distance(projection) + hero.BoundingRadius) / hero.MoveSpeed;
=======
                evadeTime = 1000 * (spell.radius - heroPos.LSDistance(projection) + hero.BoundingRadius) / hero.MoveSpeed;
>>>>>>> origin/master
                spellHitTime = spell.GetSpellHitTime(projection);
            }
            else if (spell.spellType == SpellType.Circular)
            {
<<<<<<< HEAD
                evadeTime = 1000 * (spell.radius - heroPos.Distance(spell.endPos)) / hero.MoveSpeed;
=======
                evadeTime = 1000 * (spell.radius - heroPos.LSDistance(spell.endPos)) / hero.MoveSpeed;
>>>>>>> origin/master
                spellHitTime = spell.GetSpellHitTime(heroPos);
            }

            rEvadeTime = evadeTime;
            rSpellHitTime = spellHitTime;

            return spellHitTime > evadeTime;
        }

        public static BoundingBox GetLinearSpellBoundingBox(this Spell spell)
        {
            var myBoundingRadius = ObjectCache.myHeroCache.boundingRadius;
            var spellDir = spell.direction;
            var pSpellDir = spell.direction.Perpendicular();
            var spellRadius = spell.radius;
            var spellPos = spell.currentSpellPosition - spellDir * myBoundingRadius; //leave some space at back of spell
            var endPos = spell.GetSpellEndPosition() + spellDir * myBoundingRadius; //leave some space at the front of spell

            var startRightPos = spellPos + pSpellDir * (spellRadius + myBoundingRadius);
            var endLeftPos = endPos - pSpellDir * (spellRadius + myBoundingRadius);


            return new BoundingBox(new Vector3(endLeftPos.X, endLeftPos.Y, -1), new Vector3(startRightPos.X, startRightPos.Y, 1));
        }

        public static Vector2 GetSpellEndPosition(this Spell spell)
        {
            return spell.predictedEndPos == Vector2.Zero ? spell.endPos : spell.predictedEndPos;
        }

        public static void UpdateSpellInfo(this Spell spell)
        {
            spell.currentSpellPosition = spell.GetCurrentSpellPosition();
<<<<<<< HEAD
            spell.currentNegativePosition = spell.GetCurrentSpellPosition(true, 0);
=======
            spell.currentNegativePosition = spell.GetCurrentSpellPosition(true);

>>>>>>> origin/master
            spell.dangerlevel = spell.GetSpellDangerLevel();

            if (spell.info.name == "TaricE")
            {
<<<<<<< HEAD
                var taric = EntityManager.Heroes.Enemies.FirstOrDefault(x => x.ChampionName == "Taric");
=======
                var taric = HeroManager.Enemies.FirstOrDefault(x => x.ChampionName == "Taric");
>>>>>>> origin/master
                if (taric != null)
                {
                    spell.currentSpellPosition = taric.ServerPosition.To2D();
                    spell.endPos = taric.ServerPosition.To2D() + spell.direction * spell.info.range;
                }
            }

            if (spell.info.name == "TaricE2")
            {
<<<<<<< HEAD
                var partner = EntityManager.Heroes.Enemies.FirstOrDefault(x => x.HasBuff("taricwleashactive") && x.ChampionName != "Taric");
=======
                var partner = HeroManager.Enemies.FirstOrDefault(x => x.HasBuff("taricwleashactive") && x.ChampionName != "Taric");
>>>>>>> origin/master
                if (partner != null)
                {
                    spell.currentSpellPosition = partner.ServerPosition.To2D();
                    spell.endPos = partner.ServerPosition.To2D() + spell.direction * spell.info.range;
                }
            }
        }

<<<<<<< HEAD
        public static Vector2 GetCurrentSpellPosition(this Spell spell, bool allowNegative = false, float delay = 0,
=======
        public static Vector2 GetCurrentSpellPosition(this Spell spell, bool allowNegative = false, float delay = 0, 
>>>>>>> origin/master
            float extraDistance = 0)
        {
            Vector2 spellPos = spell.startPos;

<<<<<<< HEAD
            if (spell.spellType == SpellType.Line || spell.spellType == SpellType.Arc)
=======
            if (spell.spellType == SpellType.Line
                || spell.spellType == SpellType.Arc)
>>>>>>> origin/master
            {
                float spellTime = EvadeUtils.TickCount - spell.startTime - spell.info.spellDelay;

                if (spell.info.projectileSpeed == float.MaxValue)
                    return spell.startPos;

                if (spellTime >= 0 || allowNegative)
                {
                    spellPos = spell.startPos + spell.direction * spell.info.projectileSpeed * (spellTime / 1000);
                }
            }
            else if (spell.spellType == SpellType.Circular)
            {
                spellPos = spell.endPos;
            }

            if (spell.spellObject != null && spell.spellObject.IsValid && spell.spellObject.IsVisible &&
<<<<<<< HEAD
                spell.spellObject.Position.To2D().Distance(ObjectCache.myHeroCache.serverPos2D) < spell.info.range + 1000)
=======
                spell.spellObject.Position.To2D().LSDistance(ObjectCache.myHeroCache.serverPos2D) < spell.info.range + 1000)
>>>>>>> origin/master
            {
                spellPos = spell.spellObject.Position.To2D();
            }

            if (delay > 0 && spell.info.projectileSpeed != float.MaxValue
                          && spell.spellType == SpellType.Line)
            {
                spellPos = spellPos + spell.direction * spell.info.projectileSpeed * (delay / 1000);
            }

            if (extraDistance > 0 && spell.info.projectileSpeed != float.MaxValue
                          && spell.spellType == SpellType.Line)
            {
                spellPos = spellPos + spell.direction * extraDistance;
            }

            return spellPos;
        }

        public static bool LineIntersectLinearSpell(this Spell spell, Vector2 a, Vector2 b)
        {
            var myBoundingRadius = ObjectManager.Player.BoundingRadius;
            var spellDir = spell.direction;
            var pSpellDir = spell.direction.Perpendicular();
            var spellRadius = spell.radius;
            var spellPos = spell.currentSpellPosition;// -spellDir * myBoundingRadius; //leave some space at back of spell
            var endPos = spell.GetSpellEndPosition();// +spellDir * myBoundingRadius; //leave some space at the front of spell

            var startRightPos = spellPos + pSpellDir * (spellRadius + myBoundingRadius);
            var startLeftPos = spellPos - pSpellDir * (spellRadius + myBoundingRadius);
            var endRightPos = endPos + pSpellDir * (spellRadius + myBoundingRadius);
            var endLeftPos = endPos - pSpellDir * (spellRadius + myBoundingRadius);

            bool int1 = MathUtils.CheckLineIntersection(a, b, startRightPos, startLeftPos);
            bool int2 = MathUtils.CheckLineIntersection(a, b, endRightPos, endLeftPos);
            bool int3 = MathUtils.CheckLineIntersection(a, b, startRightPos, endRightPos);
            bool int4 = MathUtils.CheckLineIntersection(a, b, startLeftPos, endLeftPos);

            if (int1 || int2 || int3 || int4)
            {
                return true;
            }

            return false;
        }

        public static bool LineIntersectLinearSpellEx(this Spell spell, Vector2 a, Vector2 b, out Vector2 intersection) //edited
        {
            var myBoundingRadius = ObjectManager.Player.BoundingRadius;
            var spellDir = spell.direction;
            var pSpellDir = spell.direction.Perpendicular();
            var spellRadius = spell.radius;
            var spellPos = spell.currentSpellPosition - spellDir * myBoundingRadius; //leave some space at back of spell
            var endPos = spell.GetSpellEndPosition() + spellDir * myBoundingRadius; //leave some space at the front of spell

            var startRightPos = spellPos + pSpellDir * (spellRadius + myBoundingRadius);
            var startLeftPos = spellPos - pSpellDir * (spellRadius + myBoundingRadius);
            var endRightPos = endPos + pSpellDir * (spellRadius + myBoundingRadius);
            var endLeftPos = endPos - pSpellDir * (spellRadius + myBoundingRadius);

<<<<<<< HEAD
            List<Geometry.IntersectionResult> intersects = new List<Geometry.IntersectionResult>();
=======
            List<EloBuddy.SDK.Geometry.IntersectionResult> intersects = new List<EloBuddy.SDK.Geometry.IntersectionResult>();
>>>>>>> origin/master
            Vector2 heroPos = ObjectManager.Player.ServerPosition.To2D();

            intersects.Add(a.Intersection(b, startRightPos, startLeftPos));
            intersects.Add(a.Intersection(b, endRightPos, endLeftPos));
            intersects.Add(a.Intersection(b, startRightPos, endRightPos));
            intersects.Add(a.Intersection(b, startLeftPos, endLeftPos));

<<<<<<< HEAD
            var sortedIntersects = intersects.Where(i => i.Intersects).OrderBy(i => i.Point.Distance(heroPos)); //Get first intersection
=======
            var sortedIntersects = intersects.Where(i => i.Intersects).OrderBy(i => i.Point.LSDistance(heroPos)); //Get first intersection
>>>>>>> origin/master

            if (sortedIntersects.Count() > 0)
            {
                intersection = sortedIntersects.First().Point;
                return true;
            }

            intersection = Vector2.Zero;
            return false;
        }

    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/master
