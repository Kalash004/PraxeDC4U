namespace SessionService.SessionTemplate_Creater
{
    /// <summary>
    /// Object for storing session id of users
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    public class SessionId
    {
        private readonly string _id;

        public SessionId(string id)
        {
            _id = id;
        }
        public string Id => _id;
        public override string ToString()
        {
            return Id;
        }
    }
}