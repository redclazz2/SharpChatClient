namespace SharpClient.Infrastructure.Model
{
    public class Transaction
    {
        public string route {get; set;}
        public object data {get; set;}

        public Transaction(string route, object data)
        {
            this.route = route;
            this.data = data;
        }
    }
}