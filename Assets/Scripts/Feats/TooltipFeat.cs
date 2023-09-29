using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipFeat : TooltipButton
{
    public void SetupFeatTooltip()
    {
        title = gameObject.name;

        switch(gameObject.name)
        {
            case "Insulation":
                description = "Years of research and you perfected your insulation armor. Now your burning damage is reduced.";
                break;
            case "Acid Vial":
                description = "Your chemistry skills allow you to always have a vial of acid prepared to throw on your enemies. Acid will decrease their energy.";
                break;
            case "Acidthrower":
                description = "Using your chemistry and engineering skills you've built an Acidthrower, a gun that showers acid in your enemies, decreasing their energy.";
                break;
            case "Flamethrower":
                description = "Using your chemistry and engineering skills you've built a Flamethrower, a gun that showers fire in your enemies, causing burning damage.";
                break;
            case "Autoturret":
                description = "Using your engineering skills you can quickly put together an Autoturret to rain lasers on your enemies.";
                break;
            case "Bacterial Fever":
                description = "Using your biology skills you discovered a bacteria that will cause an awful fever on any organic form, slowing it down and decreasing their energy.";
                break;
            case "Build Drone":
                description = "Considering you have scraps and electronic parts, you can quickly put together an improvised drone to help you in combat.";
                break;
            case "Disable Drone":
                description = "Your expertise in engineering allows you to quickly identify weak spots in drones and disable them.";
                break;
            case "Disable System":
                description = "Your expertise in engineering allows you to quickly disable entire systems as long as you have access to them.";
                break;
            case "Electrical Mine":
                description = "Your expertise in engineering allows you to quickly put together an Electrical Mine that will explode causing electrical damage to everyone around." +
                    " Electrical damage is especially hard on droids.";
                break;
            case "EMP Attack":
                description = "Using your engineering skills you created a small device that can shoot an EMP charge and disable all electrical systems of droids, drones and turrets.";
                break;
            case "Eye Bacteria":
                description = "Using your biology skills you engineered a bacteria that will affect a creature's eyes, rendering them temporarily blind.";
                break;
            case "Fire Ammo":
                description = "Using your engineering skills you engineered ammo that explodes in fire when the enemy is hit, causing burning damage.";
                break;
            case "Poison Ammo":
                description = "Using your engineering skills you engineered ammo that explodes in poison when the enemy is hit, causing poison damage.";
                break;
            case "Fire Arrow":
                description = "Using your engineering skills you engineered an arrow that is permanently set on fire, causing burning damage.";
                break;
            case "Fix Drone":
                description = "Your expertise in engineering allows you to quickly fix drones you find in the battlefield and put them to work for you.";
                break;
            case "Glide":
                description = "Using your engineering knowledge, you crafted a glider that will most likely help you not fall to your death. Just jump from a high altitude " +
                    "and trust me, I'm an engineer.";
                break;
            case "Grease Trap":
                description = "Hunting is about making your enemies fall and move slower. With this Grease Trap you created, they will rarely be on their feet.";
                break;
            case "Hack Lock":
                description = "You can hack advanced locks and it's very difficult to keep you out of a locked door.";
                break;
            case "Hack Turret":
                description = "Use your Chronowatch to quickly disrupt turrets or make them fight for you.";
                break;
            case "Holosphere":
                description = "Holograms can be used to disrupt an enemy's line of sight and even attract the less smart ones.";
                break;
            case "Nanocontrol":
                description = "Biologists have been using nanotechnology to gain control of a creature's motor and intelligent functions. This could also be used in combat." +
                    " It's highly unethical, and highly efficient.";
                break;
            case "Nitrobomb":
                description = "These bombs will explode with liquid nitrogen, immediately freezing everyone around them. Then you can break them into pieces.";
                break;
            case "Organic Transmutation":
                description = "Another highly unethical practice in the field of biology is altering someone's DNA to quickly change their physical form. This could be used " +
                    "to turn you into a combat creature or your enemy into a space chicken.";
                break;
            case "Poison Arrow":
                description = "You found a way to create an arrow filled with a small quantity of poison that ejects the liquid once it penetrates the skin. Poisoned enemies will take " +
                    "damage over time, but it doesn't affect droids obviously.";
                break;
            case "Poison Shot":
                description = "You found a way to create a small dart filled with a large quantity of poison that ejects the liquid once it penetrates the skin. Poisoned enemies will take " +
                    "damage over time, but it doesn't affect droids obviously.";
                break;
            case "Poison Bottle":
                description = "Your skills in biology allow you to quickly produce a potent poison that can be used in drinks or simply be thrown at enemies.";
                break;
            case "Regeneration":
                description = "Your skills in biology allow you to create a component that will regenerate tissue and nerves, removing physical damage caused by combat over time.";
                break;
            case "Repair System":
                description = "Your engineering expertise allow you to repair systems provided you have access to them.";
                break;
            case "Reprogram Droid":
                description = "Your engineering expertise allow you to reprogram droids quickly, even in combat, and turn them on each other.";
                break;
            case "Tripwire":
                description = "Using your engineering expertise, you devised a tripwire that will explode when activated by anyone.";
                break;
            case "Viral Charge":
                description = "Your biology skills allow you to create a dangerous virus that will cause damage in skin tissue and spread to nearby enemies if airborne.";
                break;
            case "Wolf Trap":
                description = "The name of this trap comes from the Wolf, an animal brought from Earth, now evolved into the Voidstalkers. These traps are good against both " +
                    "and any other creature of the same size. Larger creatures will require a larger trap.";
                break;

            default:
                break;
        }
    }
}
