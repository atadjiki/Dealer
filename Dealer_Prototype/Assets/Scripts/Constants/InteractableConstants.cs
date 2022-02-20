namespace Constants
{
    public class InteractableConstants
    {
        public enum InteractionState { None, Busy, Available }

        public enum InteractableID
        {
            Door,
            Generic,
            Chair,
            Television,
            None,
        }

        public enum InteractionContext
        {
            Select,
            Deselect,
            Move,
            Approach,
            Interact,
            Sit,
            None
        };

        public static InteractableConstants.InteractionContext GetContextByInteractableID(Interactable interactable)
        {
            if (interactable.GetID() == Constants.InteractableConstants.InteractableID.Generic.ToString())
            {
                return InteractableConstants.InteractionContext.Interact;
            }
            else if (interactable.GetID() == Constants.InteractableConstants.InteractableID.Chair.ToString())
            {
                return InteractableConstants.InteractionContext.Sit;
            }
            else if (interactable.GetID() == Constants.InteractableConstants.InteractableID.Television.ToString())
            {
                return InteractableConstants.InteractionContext.Interact;
            }
            else
            {
                return InteractableConstants.InteractionContext.None;
            }
        }

        public static string GetInteractionTipTextContext(InteractableConstants.InteractionContext context)
        {

            string text = "";

            switch (context)
            {
                case InteractableConstants.InteractionContext.Select:
                    text = "Select";
                    break;
                case InteractableConstants.InteractionContext.Deselect:
                    text = "Deselect";
                    break;
                case InteractableConstants.InteractionContext.Move:
                    text = "Move";
                    break;
                case InteractableConstants.InteractionContext.Interact:
                    text = "Interact";
                    break;
                case InteractableConstants.InteractionContext.Approach:
                    text = "Approach";
                    break;
                case InteractableConstants.InteractionContext.Sit:
                    text = "Sit";
                    break;
            }

            return text;
        }
    }
}
 