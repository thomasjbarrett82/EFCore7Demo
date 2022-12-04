namespace EFCore7Demo.Models;

public class ChangeLog {
    public int Id { get; set; }
    public string TableName { get; set; }
    public int TableKey { get; set; }
    public List<ChangeDetail> ChangeDetails { get; set; }
    public string ChangeType { get; set; }
    public DateTime ChangedOn { get; set; }
}
