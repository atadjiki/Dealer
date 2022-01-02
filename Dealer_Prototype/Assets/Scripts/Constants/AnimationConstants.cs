namespace Constants
{
    public class AnimationConstants
    {
        public enum Animations
        {
            Idle, Talking, Walking, ButtonPush, Sitting_Idle
        }
        
        public static string Idle = "Idle";
        public static string Talking = "Talking";
        public static string Walking = "Walking";
        public static string ButtonPush = "ButtonPush";
        public static string Sitting_Idle = "Sitting_Idle";

        public static string GetAnimByEnum(Animations anim)
        {
            if (anim == Animations.Idle)
            {
                return Idle;
            }
            else if (anim == Animations.Talking)
            {
                return Talking;
            }
            else if (anim == Animations.Walking)
            {
                return Walking;
            }
            else if (anim == Animations.ButtonPush)
            {
                return ButtonPush;
            }
            else if (anim == Animations.Sitting_Idle)
            {
                return Sitting_Idle;
            }
            else
            {
                return Idle;
            }
        }
    }
}