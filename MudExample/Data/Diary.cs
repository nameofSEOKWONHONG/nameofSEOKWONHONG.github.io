using Magic.IndexedDb;
using Magic.IndexedDb.Interfaces;
using Magic.IndexedDb.SchemaAnnotations;

namespace MudExample.Data;

public class IndexDbContext : IMagicRepository
{
    public static readonly IndexedDbSet Diary = new("Diary");
}

public class Diary : MagicTableTool<Diary>, IMagicTable<Diary.DbSets>
{
    public sealed class DbSets
    {
        public readonly IndexedDbSet Diary = IndexDbContext.Diary;
    }

    [MagicName("_id")]
    public int _Id { get; set; }

    [MagicName("title")]
    public string Title { get; set; }
    [MagicName("content")]
    public string Content { get; set; }
    [MagicName("createdDate")]
    public DateTime CreatedDate { get; set; }

    public DbSets Databases { get; } = new();

    public string GetTableName() => "Diary";

    public List<IMagicCompoundIndex> GetCompoundIndexes() =>
        new List<IMagicCompoundIndex>() {
            CreateCompoundIndex(x => x.Title)
        };

    public IMagicCompoundKey GetKeys() =>
        CreatePrimaryKey(x => x._Id, true);

    public IndexedDbSet GetDefaultDatabase() => IndexDbContext.Diary;
}