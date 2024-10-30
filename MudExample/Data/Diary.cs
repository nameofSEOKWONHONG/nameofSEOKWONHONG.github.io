using Magic.IndexedDb;
using Magic.IndexedDb.SchemaAnnotations;

namespace MudExample.Data;

[MagicTable(nameof(Diary), "MyDatabase")]
public class Diary
{
    [MagicPrimaryKey("id")]
    public int _id { get; set; }
    [MagicIndex]
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
}