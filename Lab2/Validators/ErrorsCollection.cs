using System.Collections.Generic;

namespace Lab2.Servies
{
    public class ErrorsCollection
    {
        public ErrorsCollection()
        {
            ErrorMessages = new List<string>();
        }
        public string Entity { get; set; }
        public List<string> ErrorMessages { get; set; }
        //public T CorrectData { get; set; }
    }
}