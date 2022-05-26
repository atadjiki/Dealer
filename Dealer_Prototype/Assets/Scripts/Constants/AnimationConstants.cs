namespace Constants
{
    public class AnimationConstants
    {

        public enum Anim
        { 
            Idle,
            Idle_Drunk,

            Sitting_Default,
            Walking,

            Talking_Default,

            Emote_Headshake_Annoyed,
            Emote_Headshake_Thoughtful,

            Emote_Headnod_Yes,
            Emote_Headnod_Lengthy,
            Emote_Headnod_Hard,
            Emote_Headnod_Sarcastic,

            Emote_Hand_Happy,

            Emote_Acknowledging,
            Emote_Angry,
            Emote_Being_Cocky,
            Emote_Dismissing,
            Emote_Look_Away,
            Emote_Sigh_Relieved,
            Emote_WeightShift,

            Interaction_ButtonPush
        }

        //sitting
        private const string Sitting_Default = "Sitting_Default";

        //idles
        private const string Idle_Male = "Idle_Male";
        private const string Idle_Female = "Idle_Female";
        private const string Idle_Drunk = "Idle_Drunk";

        //walking
        private const string Walking_Male = "Walking_Male";
        private const string Walking_Female = "Walking_Female";

        //talking
        private const string Talking_Default = "Talking_Default";

        //emote
        private const string Emote_Headshake_Annoyed = "Emote_Headshake_Annoyed";
        private const string Emote_Headshake_Thoughtful = "Emote_Headshake_Thoughtful";


        private const string Emote_Headnod_Yes = "Emote_Headnod_Yes";
        private const string Emote_Headnod_Lengthy = "Emote_Headnod_Lengthy";
        private const string Emote_Headnod_Hard = "Emote_Headnod_Hard";
        private const string Emote_Headnod_Sarcastic = "Emote_Headnod_Sarcastic";

        private const string Emote_Hand_Happy = "Emote_Hand_Happy";

        private const string Emote_Acknowledging = "Emote_Acknowledging";
        private const string Emote_Angry = "Emote_Angry";
        private const string Emote_Being_Cocky = "Emote_Being_Cocky";
        private const string Emote_Dismissing = "Emote_Dismissing";
        private const string Emote_Look_Away = "Emote_Look_Away";
        private const string Emote_Sigh_Relieved = "Emote_Sigh_Relieved";
        private const string Emote_WeightShift = "Emote_WeightShift";

        //interaction
        private const string Interaction_ButtonPush = "Interaction_ButtonPush";

        public static string FetchAnimString(CharacterConstants.CharacterID ID, Anim anim)
        {
            CharacterConstants.GenderType gender = Constants.CharacterConstants.GetGenderBYID(ID);

            switch(anim)
            {
                case Anim.Emote_Acknowledging:
                    return Emote_Acknowledging;
                case Anim.Emote_Angry:
                    return Emote_Angry;
                case Anim.Emote_Being_Cocky:
                    return Emote_Being_Cocky;
                case Anim.Emote_Dismissing:
                    return Emote_Dismissing;
                case Anim.Emote_Hand_Happy:
                    return Emote_Hand_Happy;
                case Anim.Emote_Headnod_Hard:
                    return Emote_Headnod_Hard;
                case Anim.Emote_Headnod_Lengthy:
                    return Emote_Headnod_Lengthy;
                case Anim.Emote_Headnod_Sarcastic:
                    return Emote_Headnod_Sarcastic;
                case Anim.Emote_Headnod_Yes:
                    return Emote_Headnod_Yes;
                case Anim.Emote_Headshake_Annoyed:
                    return Emote_Headshake_Annoyed;
                case Anim.Emote_Headshake_Thoughtful:
                    return Emote_Headshake_Thoughtful;
                case Anim.Emote_Look_Away:
                    return Emote_Look_Away;
                case Anim.Emote_Sigh_Relieved:
                    return Emote_Sigh_Relieved;
                case Anim.Emote_WeightShift:
                    return Emote_WeightShift;

                case Anim.Idle:
                    if(gender == CharacterConstants.GenderType.Male)
                        return Idle_Male;
                    else 
                        return Idle_Female;
                case Anim.Idle_Drunk:
                    return Idle_Drunk;

                case Anim.Interaction_ButtonPush:
                    return Interaction_ButtonPush;

                case Anim.Sitting_Default:
                    return Sitting_Default;

                case Anim.Talking_Default:
                    return Talking_Default;

                case Anim.Walking:
                    if (gender == CharacterConstants.GenderType.Male)
                        return Walking_Male;
                    else
                        return Walking_Female;

            }

            return "";
        }
    }
}