namespace DataAccessLibrary.Models
{
    public class ReturnData<T,K>
    {
        private T result;
        private K message;

        public ReturnData(T result, K message)
        {
            Result = result;
            Message = message;
        }

        public T Result { get => result; set => result = value; }
        public K Message { get => message; set => message = value; }

    }
}