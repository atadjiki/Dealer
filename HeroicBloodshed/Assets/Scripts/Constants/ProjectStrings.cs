using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public static string GetDisplayString(CharacterEvent characterEvent)
    {
        switch(characterEvent)
        {
            case CharacterEvent.ABILITY:
                return "Perform Ability";
            case CharacterEvent.DAMAGE:
                return "Receive Damage";
            case CharacterEvent.HIT:
                return "Hit";
            case CharacterEvent.KILLED:
                return "Kill";
            case CharacterEvent.SELECTED:
                return "Select";
            case CharacterEvent.DESELECTED:
                return "Deselect";
            case CharacterEvent.TARGETED:
                return "Targeted";
            case CharacterEvent.UNTARGETED:
                return "Untargeted";
            default:
                return characterEvent.ToString();
        }
    }

    public static string GetDisplayString(WeaponID weapon)
    {
        switch (weapon)
        {
            case WeaponID.Pistol:
                return "Pistol";
            case WeaponID.Revolver:
                return "Revolver";
            case WeaponID.SMG:
                return "Machine-Pistol";
            default:
                return weapon.ToString();
        }
    }

    public static string GetDisplayString(CharacterID characterID)
    {
        switch (characterID)
        {
            case CharacterID.HENCHMAN:
                return "Henchman";
            case CharacterID.GOON:
                return "Goon";
            case CharacterID.PLAYER_1:
                return "Mulder";
            case CharacterID.PLAYER_2:
                return "Scully";
            default:
                return characterID.ToString();
        }
    }

    public static string GetDisplayString(EncounterState state)
    {
        switch (state)
        {
            case EncounterState.INIT:
                return "Initializing...";
            case EncounterState.SETUP_COMPLETE:
                return "Setup Complete";
            case EncounterState.BUILD_QUEUES:
                return "Building Queues...";
            case EncounterState.CHECK_CONDITIONS:
                return "Checking Conditions...";
            case EncounterState.DONE:
                return "Done";
            case EncounterState.SELECT_CURRENT_CHARACTER:
                return "Selecting Next Character...";
            case EncounterState.CHOOSE_ACTION:
                return "Choosing action...";
            case EncounterState.CHOOSE_TARGET:
                return "Choosing target";
            case EncounterState.CANCEL_ACTION:
                return "Cancelling target";
            case EncounterState.PERFORM_ACTION:
                return "Performing Action...";
            case EncounterState.DESELECT_CURRENT_CHARACTER:
                return "Deselecting Current Character...";
            case EncounterState.UPDATE:
                return "Updating...";
            default:
                return string.Empty;

        }
    }

    public static string GetDisplayString(AbilityID abilityID)
    {
        switch (abilityID)
        {
            case AbilityID.Attack:
                return "Attack";
            case AbilityID.Heal:
                return "Heal";
            case AbilityID.Reload:
                return "Reload";
            case AbilityID.SkipTurn:
                return "Skip Turn";
            default:
                return abilityID.ToString();
        }
    }

    public static string GetEventString(AbilityID abilityID)
    {
        switch (abilityID)
        {
            case AbilityID.Attack:
                return "Attacking";
            case AbilityID.Heal:
                return "Healing";
            case AbilityID.Reload:
                return "Reloading";
            case AbilityID.SkipTurn:
                return "Skipping Turn";
            default:
                return abilityID.ToString();
        }
    }
}
