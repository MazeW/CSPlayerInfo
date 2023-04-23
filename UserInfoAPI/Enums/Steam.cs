namespace UserInfoAPI.Enums
{
    public class Steam // might have to rename to avoid name colission 
    {
        public enum CommunityVisibilityState
        {
            Invalid,
            Private,
            FriendsOnly,
            Public
        }
        public enum ProfileState
        {
            NewAccount,
            CommunityProfileCreated
        }

        public enum CommentPermission
        {
            Private,
            FriendsOnly, // i think, need to verify
            Public
        }
        public enum PersonaState
        {
            Offline,
            Online,
            Busy,
            Away,
            Snooze,
            LookingToTrade,
            LookingToPlay
        }
    }
}
