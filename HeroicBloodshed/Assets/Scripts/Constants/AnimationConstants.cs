namespace Constants
{
    public class Anim
    {
        public enum State
        {
            Idle,
            Walking,
            Running,
            Attack_Single,
        }

        public enum ID
        {
            //for cutscenes/non combat use

            //locomotion 
            Anim_Character_Idle,
            Anim_Character_Walking,
            Anim_Character_Running,

            //combat

            //pistol/handgun
            Anim_Character_Pistol_Idle,
            Anim_Character_Pistol_Walking,
            Anim_Character_Pistol_Running,
            Anim_Character_Pistol_Attack_Single,
        }

        public static ID GetAnimByWeaponType(Game.WeaponID Weapon, State state)
        {
            switch(Weapon)
            {
                case Game.WeaponID.Pistol:
                case Game.WeaponID.Revolver:
                    return GetPistolAnim(state);

                default:
                    return ID.Anim_Character_Idle;
            }
        }

        public static ID GetPistolAnim(State state)
        {
            switch(state)
            {
                case State.Idle:
                    return ID.Anim_Character_Pistol_Idle;
                case State.Walking:
                    return ID.Anim_Character_Pistol_Walking;
                case State.Running:
                    return ID.Anim_Character_Pistol_Running;
                case State.Attack_Single:
                    return ID.Anim_Character_Pistol_Attack_Single;

                default:
                    return ID.Anim_Character_Pistol_Idle;
            }

        }

    }
}