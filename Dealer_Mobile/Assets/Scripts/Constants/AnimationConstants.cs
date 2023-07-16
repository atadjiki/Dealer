namespace Constants
{
    public class AnimationConstants
    {
        public enum State
        {
            Idle,
            Walking,
            Running,
            Attack_Single,
        }

        public enum Anim
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

        public static Anim GetAnimByWeaponType(CharacterConstants.Weapon Weapon, State state)
        {
            switch(Weapon)
            {
                case CharacterConstants.Weapon.Pistol:
                case CharacterConstants.Weapon.Revolver:
                    return GetPistolAnim(state);

                default:
                    return Anim.Anim_Character_Idle;
            }
        }

        public static Anim GetPistolAnim(State state)
        {
            switch(state)
            {
                case State.Idle:
                    return Anim.Anim_Character_Pistol_Idle;
                case State.Walking:
                    return Anim.Anim_Character_Pistol_Walking;
                case State.Running:
                    return Anim.Anim_Character_Pistol_Running;
                case State.Attack_Single:
                    return Anim.Anim_Character_Pistol_Attack_Single;

                default:
                    return Anim.Anim_Character_Pistol_Idle;
            }

        }

    }
}