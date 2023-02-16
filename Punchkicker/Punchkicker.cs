using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using static ItemDrop;
using static Skills;

namespace Punchkicker
{
    [BepInPlugin("pursuedbycosplay.Punchkicker", "Punchkicker", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class ValheimMod : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("pursuedbycosplay.Punchkicker");

        void Awake()
        {
            harmony.PatchAll();
        }


        [HarmonyPatch(typeof(Humanoid), "GetCurrentWeapon")]
        public static class ModifyCurrentWeapon
        {
            private static ItemData Postfix(ItemData __weapon, ref Character __instance)
            {
                if (__weapon != null && __weapon.m_shared.m_name == "Unarmed" && __instance.InAttack())
                {
                    Player val = (Player)__instance;
                    List<ItemData> list = val.GetInventory().GetEquipedtems();
                    float legarmor = 1f;
                    foreach (ItemData item in list)
                    {
                        if (item.m_shared.m_name.Contains("legs"))
                        {
                            legarmor = item.GetArmor();
                            break;
                        }
                    }
                    __weapon.m_shared.m_backstabBonus= 3f;
                    __weapon.m_shared.m_damages.m_blunt = (val.GetSkillFactor((SkillType)11) * 50 + (2.5f*legarmor));
                    if (__weapon.m_shared.m_damages.m_blunt <= 2f)
                    {
                        __weapon.m_shared.m_damages.m_blunt = 2f;
                    }
                }
                return __weapon;
            }
        }
    }
}
