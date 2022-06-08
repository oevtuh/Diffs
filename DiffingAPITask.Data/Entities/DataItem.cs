namespace DiffingAPITask.Data.Entities
{
    public class DataItem : BaseEntity
    {
        public string Left { get; set; }
        public string Right { get; set; }

        public bool IsReady()
        {
            return !string.IsNullOrEmpty(Left) && !string.IsNullOrEmpty(Right);
        }
    }
}