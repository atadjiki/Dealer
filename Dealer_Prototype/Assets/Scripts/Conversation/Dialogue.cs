using Constants;

public class Dialogue 
{
    public CharacterComponent Speaker;
    public string Text;
    public AnimationConstants.Anim Emote;
    public float Duration;

    public Dialogue(CharacterComponent speaker, string text, AnimationConstants.Anim emote, float duration)
    {
        Speaker = speaker;
        Text = text;
        Emote = emote;
        Duration = duration;
    }
}
