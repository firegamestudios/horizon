using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatTooltipButton : TooltipButton
{
    public void SetupThisTooltip()
    {
        string myName = gameObject.name;

        title = myName;

        switch (myName)
        {
            case "Acid Vial":
                description = "You always carry acid vials with you. This potent acid can corrode armor and lower your enemy's resistances.";
                break;
            case "Acidthrower":
                description = "Your expertise in engineering allowed you to build an Acidthrower, similar to a Flamethrower but it spits acid to lower your enemy's resistance";
                break;
            case "Autoturret":
                description = "No engineer is complete without an Autoturret. You can quickly put it together in the battlefield to anihilate your enemies.";
                break;
            case "Bacterial Fever":
                description = "This vial is full with a dangerous bacteria that eats flesh and gives organic enemies a fever that slows them down. Just throw and let the little guys do the work for you.";
                break;
            case "Flamethrower":
                description = "A sophisticated flamethrower that causes burning damage over time. Why not add more damage to the damage you're already doing?";
                break;
            case "Glider":
                description = "This glider is perfectly safe. Just drop from a high altitude and trust me: I'm an engineer.";
                break;
            case "Holosphere":
                description = "A hologram that will attract your enemies attacks temporarily.";
                break;
            case "Nitrobomb":
                description = "Liquid Nitrogen is always so sexy.Throw this baby and watch your enemies break into pieces.";
                break;
            case "Insulation":
                description = "Your research paid off and you found out a good way to insulate yourself against fire damage.";
                break;
            case "Tripwire":
                description = "A tripwire connected to explosives - a classic. Just prepare the trap and watch your enemies blow to pieces.";
                break;
            case "Eye Bacteria":
                description = "You found a special kind of bacteria that eats the nerves in the eyes. Your enemies will be blind and won't be able to aim properly at you.";
                break;
            case "Nanocontrol":
                description = "Nanotechnology at your service to take control of your enemies motor functions and make them fight for you.";
                break;
            case "Organic Transmutation":
                description = "A little research into several alien genomas led to a discovery that allows you to alter your own and become a formidable creature with increased strength and speed.";
                break;
            case "Poison Shot":
                description = "Your research revelead a poisonous plant that can seriously damage nerve tissue in organic enemies. It doesn't work on droids and machines, though.";
                break;
            case "Regeneration":
                description = "Advanced medicine allows you to use nanotechnology to quickly heal tissue and repair organic damage.";
                break;
            case "Viral Charge":
                description = "Nothing like a dangerous virus to debilitate your enemies. They will be weaker and slower with this virus.";
                break;
            case "Electrical Mine":
                description = "You are prepared with Electrical Mines that make excellent traps against fast enemies. The electricity will stun them and you can finish the job.";
                break;
            case "Fire Ammo":
                description = "Specialized ammo for your laser rifle or pistol. This alteration will add burning damage to the damage you're already doing out there.";
                break;
            case "Flaming Arrow":
                description = "Specialized arrows for your bow. This alteration will add burning damage to the damage you're already doing out there.";
                break;
            case "Grease Trap":
                description = "Prepare a trap that will spread grease on the ground so your enemies have a hard time just trying to stand up.";
                break;
            case "Nitro Trap":
                description = "A trap with liquid nitrogen to freeze your enemies and give you a chance to break them into pieces.";
                break;
            case "Poison Ammo":
                description = "Specialized ammo for your laser rifle or pistol. This alteration will add poison damage to the damage you're already doing out there.";
                break;
            case "Poison Arrow":
                description = "Specialized arrows for your bow. This alteration will add poison damage to the damage you're already doing out there.";
                break;
            case "Wolf Trap":
                description = "Back on Earth, there was a creature called \"wolf\". Humans used this trap to... well, trap the wolf. It's an old concept, but works.";
                break;
            case "Build Drone":
                description = "Provided you find some scraps and electronic parts, you can quickly put a drone together to fight by your side.";
                break;
            case "Disable Drone":
                description = "Your technical expertise allows you to identify weak spots and immediately disable drones.";
                break;
            case "Disable System":
                description = "There is no code too complicated and no system too difficult for you to completely disable.";
                break;
            case "EMP Attack":
                description = "This EMP Attack will damage any electronic circuit - drone or droid.";
                break;
            case "Fix Drone":
                description = "Broken drone? Not while you are around. Fix any drone to fight for you or give you information.";
                break;
            case "Hack Lock":
                description = "There's no electronic lock you can't hack. It's nearly impossible for you to fail opening an electronic lock.";
                break;
            case "Hack Turret":
                description = "Approach a turret and quickly reprogram it to fight for you and not against you.";
                break;
            case "Repair System":
                description = "Fix any broken system to retrieve information, open locks or simply access its functions.";
                break;
            case "Reprogram Droid":
                description = "Even during combat, you are quick to reprogram droids and make them think you are their ally instead of enemy.";
                break;
            default:
                break;
        }
    }
}
