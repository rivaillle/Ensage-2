using System;
using System.Linq;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using Ensage.Common.Menu;
using SharpDX;
using System.Windows.Input;

namespace Bristleback_Sharp
{
    internal class Program
    {
        private static Ability Quill, Goo;
        private static Hero _source, _target;
        private static Item abyssal, solar, medallion, halberd, atos, dust;
        private static bool chase;
        private static readonly uint[] Quilldmg = { 20, 40, 60, 80 };
        private static readonly Menu Menu = new Menu("Bristleback", "bristle", true);
        static void Main(string[] args)
        {
            Game.OnUpdate += Game_OnUpdate;
            Game.OnWndProc += Game_OnWndProc;
            Game.PrintMessage("Bristleback Sharp by <font color='#ff1111'>Spyware293</font> Loaded !!", MessageType.LogMessage);
            var menu_utama = new Menu("Options", "opsi");
            menu_utama.AddItem(new MenuItem("Quill", "Quill").SetValue(new StringList(new[] { "Max", "Smart", "Disable","Farm" })));
            menu_utama.AddItem(new MenuItem("enable", "enable").SetValue(true));
            Menu.AddSubMenu(menu_utama);
            Menu.AddToMainMenu();
        }
        private static void Game_OnWndProc (WndEventArgs args)
        {
            if (!Game.IsChatOpen)
            {
                if (Game.IsKeyDown(32))
                {
                    chase = false;
                }
                else
                {
                    chase = false;
                }
            }
        }
        private static float GetDistance2D(Vector3 hero, Vector3 enemy)
        {
            return (float)Math.Sqrt(Math.Pow(hero.X - enemy.X, 2) + Math.Pow(hero.Y - enemy.Y, 2));
        }
        public static void Game_OnUpdate(EventArgs args)
        {
            _source = ObjectMgr.LocalHero;
           
            if (!Game.IsInGame||Game.IsPaused||Game.IsWatchingGame)
            {
                return;
            }
            if (_source.ClassID != ClassID.CDOTA_Unit_Hero_Bristleback)
            {
                return;
            }
            var _enemy = ObjectMgr.GetEntities<Hero>().Where(hero => hero.IsAlive && !hero.IsIllusion && hero.IsVisible && hero.Team != _source.Team);
            var _creep = ObjectMgr.GetEntities<Creep>().Where(x => (x.ClassID == ClassID.CDOTA_BaseNPC_Creep_Lane || x.ClassID == ClassID.CDOTA_BaseNPC_Creep_Siege) && x.IsAlive && x.IsSpawned && x.IsVisible).ToList();
            if (_source == null)
            {
                return;
            }
            if (Quill == null)
            {
                Quill = _source.Spellbook.Spell2;
            }
            if (Goo == null)
            {
                Goo = _source.Spellbook.Spell1;
            }
            if (abyssal == null)
            {
                abyssal = _source.FindItem("item_abyssal_blade");
            }
            if (dust == null)
            {
                dust = _source.FindItem("item_dust");
            }
            if (atos == null)
            {
                atos = _source.FindItem("item_rod_of_atos");
            }
            if (solar == null)
            {
                solar = _source.FindItem("item_solar_crest");
            }
            if (medallion == null)
            {
                medallion = _source.FindItem("item_medallion_of_courage");
            }
            if (halberd == null)
            {
                halberd = _source.FindItem("item_heavens_halberd");
            }
            if (Menu.Item("Quill").GetValue<StringList>().SelectedIndex == 3 && Quill.CanBeCasted() && _source.CanCast() && Utils.SleepCheck("quill") && !_source.IsChanneling() && !_source.IsInvisible())
            {
                
                foreach (var x in _creep)
                {
                    if (x.Team == _source.GetEnemyTeam() && x.Health > 0 && x.Health < (Quilldmg[Quill.Level-1] * (1-x.DamageResist)+20) && GetDistance2D(x.Position, _source.Position) < Quill.CastRange && Utils.SleepCheck("quill"))
                    {
                        Quill.UseAbility();
                        Utils.Sleep(150 + Game.Ping, "quill");
                    }
                }
            }
            if (Menu.Item("Quill").GetValue<StringList>().SelectedIndex == 0 && Quill.CanBeCasted() && _source.CanCast() && Utils.SleepCheck("quill") && !_source.IsChanneling() && !_source.IsInvisible())
            {
                Quill.UseAbility();
                Utils.Sleep(150 + Game.Ping, "quill");
            }
            if (Menu.Item("Quill").GetValue<StringList>().SelectedIndex == 1 && Quill.CanBeCasted() && _source.CanCast() && Utils.SleepCheck("quill") && !_source.IsChanneling() && !_source.IsInvisible())
            {
                foreach (var enemy in _enemy)
                {
                    if (GetDistance2D(enemy.Position, _source.Position) < Quill.CastRange)
                    {
                        Quill.UseAbility();
                        Utils.Sleep(150 + Game.Ping, "quill");
                    }
                }
                
            }
            if (chase && Menu.Item("enable").GetValue<bool>())
            {
                _target = _source.ClosestToMouseTarget(1000);
                if (_source.CanAttack() && _source.CanCast())
                {
                    var linken = _target.Modifiers.Any(x => x.Name == "modifier_item_spheretarget") || _target.Inventory.Items.Any(x => x.Name == "item_sphere");
                    if (abyssal != null && abyssal.CanBeCasted() && Utils.SleepCheck("item_abyssal") && !linken)
                    {
                        abyssal.UseAbility(_target);
                        Utils.Sleep(400 + Game.Ping, "item_abyssal");
                    }
                    if (abyssal != null)
                    {
                        Utils.ChainStun(_source, 310, null, false);
                    }
                    if (medallion != null && medallion.CanBeCasted() && Utils.SleepCheck("item_medal") && !linken)
                    {
                        medallion.UseAbility(_target);
                        Utils.Sleep(150 + Game.Ping, "item_medal");
                    }
                    if (atos != null && atos.CanBeCasted() && Utils.SleepCheck("item_atos") && !linken)
                    {
                        medallion.UseAbility(_target);
                        Utils.Sleep(150 + Game.Ping, "item_atos");
                    }
                    if (solar != null && solar.CanBeCasted() && Utils.SleepCheck("item_solar") && !linken)
                    {
                        solar.UseAbility(_target);
                        Utils.Sleep(200 + Game.Ping, "item_solar");
                    }
                    if (dust != null && dust.CanBeCasted() && (_target.CanGoInvis() || _target.IsInvisible()) && Utils.SleepCheck("dust"))
                    {
                        dust.UseAbility();
                        Utils.Sleep(200 + Game.Ping, "dust");
                    }
                    if (Goo.CanBeCasted() && _source.CanAttack() && !_target.IsInvul() && Utils.SleepCheck("Goo"))
                    {
                        if (_source.HasItem(ClassID.CDOTA_Item_UltimateScepter))
                        {
                            if (GetDistance2D(_target.Position, _source.Position) < Goo.CastRange)
                            {
                                Goo.UseAbility();
                                Utils.Sleep(150 + Game.Ping, "Goo");
                            }

                        }
                        else
                        {
                            Goo.UseAbility(_target);
                            Utils.Sleep(150 + Game.Ping, "Goo");
                        }
                    }
                    if (!Goo.CanBeCasted() && Utils.SleepCheck("animationatk"))
                    {
                        _source.Attack(_target);
                        Utils.Sleep(Game.Ping + 150, "animationatk");
                    }
                    if (Quill.CanBeCasted() && Utils.SleepCheck("quill"))
                    {
                        Quill.UseAbility();
                        Utils.Sleep(150 + Game.Ping, "quill");
                    }
                }
                else
                {
                    if(Utils.SleepCheck("atk"))
                    _source.Attack(_target);
                    Utils.Sleep(1000, "atk");
                }
            }
        }
    }
}
