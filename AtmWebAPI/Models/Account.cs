namespace AtmWebAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public int FiftyNotes { get; set; }
        public int TwentyNotes { get; set; }
        public int TenNotes { get; set; }

       /* public Account()
        {
            FiftyNotes = 0;
            TwentyNotes = 0;
            TenNotes = 0;
        }*/
    }
}
