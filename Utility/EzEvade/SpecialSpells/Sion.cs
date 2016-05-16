using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
<<<<<<< HEAD
=======
using LeagueSharp.Common;
>>>>>>> origin/master

namespace ezEvade.SpecialSpells
{
    class Sion : ChampionPlugin
    {
        static Sion()
        {

        }

        public void LoadSpecialSpell(SpellData spellData)
        {
            /*if (spellData.spellName == "SionE")
            {
                SpellDetector.OnProcessSpecialSpell += ProcessSpell_SionE;
            }*/
        }

        private static void ProcessSpell_SionE(Obj_AI_Base hero, GameObjectProcessSpellCastEventArgs args, SpellData spellData,
            SpecialSpellEventArgs specialSpellArgs)
        {
            if (spellData.spellName == "SionE")
            {
<<<<<<< HEAD
                var objList = new List<Obj_AI_Minion>();
                foreach (var obj in ObjectManager.Get<Obj_AI_Minion>())
                {
                    if (obj != null && obj.IsValid && !obj.IsDead && obj.IsAlly)
                    {
                        objList.Add(obj);
                    }
                }

                objList.OrderBy(o => o.Distance(hero.ServerPosition));
=======
                var objList = ObjectManager.Get<Obj_AI_Minion>().Where(obj => obj != null && obj.IsValid && !obj.IsDead && obj.IsAlly).ToList();

                objList.OrderBy(o => o.LSDistance(hero.ServerPosition));
>>>>>>> origin/master

                var spellStart = args.Start.To2D();
                var dir = (args.End.To2D() - spellStart).Normalized();
                var spellEnd = spellStart + dir * spellData.range;

                foreach (var obj in objList)
                {
                    var objProjection = obj.ServerPosition.To2D().ProjectOn(spellStart, spellEnd);

<<<<<<< HEAD
                    if (objProjection.IsOnSegment && objProjection.SegmentPoint.Distance(obj.ServerPosition.To2D()) < obj.BoundingRadius + spellData.radius)
=======
                    if (objProjection.IsOnSegment && objProjection.SegmentPoint.LSDistance(obj.ServerPosition.To2D()) < obj.BoundingRadius + spellData.radius)
>>>>>>> origin/master
                    {
                        //sth happens
                    }
                }


                //specialSpellArgs.noProcess = true;
            }
        }
    }
}
