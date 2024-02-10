namespace L3T.Infrastructure.Helpers.CommonModel
{
    public class GlobalApplicationException : Exception
    {

        public GlobalApplicationException()
        {
        }

        public GlobalApplicationException(string message)
             : base(message)
        {
        }

        public GlobalApplicationException(string message, Exception inner)
             : base(message, inner)
        {
        } 

    }
}
