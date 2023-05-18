namespace SessionService.SessionTemplate_Creater
{
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