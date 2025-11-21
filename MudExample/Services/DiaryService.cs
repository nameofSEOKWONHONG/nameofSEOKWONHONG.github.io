//using Blazor.SubtleCrypto;
//using Blazored.LocalStorage;
//using eXtensionSharp;
//using Magic.IndexedDb;
//using MudExample.Data;

//namespace MudExample.Services;

//public interface IDiaryService
//{
//    Task<List<Diary>> GetDiaries();
//    Task<List<Diary>> GetDiaries(string title);
//    Task<Diary> GetDiary(int id);
//    Task<bool> SaveDiary(Diary diary);
//    Task<bool> DeleteDiary(int id);
//}

//public class DiaryService : IDiaryService
//{
//    private readonly IMagicIndexedDb _indexedDb;
//    private readonly ICryptoService _cryptoService;
    
//    private const string _diaryIndexName = "my_memories_make_me_exist";
    
//    public DiaryService(IMagicIndexedDb indexedDb, ICryptoService cryptoService)
//    {
//        _indexedDb = indexedDb;
//        _cryptoService = cryptoService;
//    }

//    public async Task<List<Diary>> GetDiaries()
//    {
//        var query = await _indexedDb.Query<Diary>();
//        var items = await query.ToListAsync();
//        foreach (var diary in items)
//        {
//            diary.Content = await _cryptoService.DecryptAsync(diary.Content);
//        }
//        return items.ToList();
//    }
    
//    public async Task<List<Diary>> GetDiaries(string title)
//    {
//        var query = await _indexedDb.Query<Diary>();
//        var items = await query.ToListAsync();
//        foreach (var diary in items)
//        {
//            diary.Content = await _cryptoService.DecryptAsync(diary.Content);
//        }
//        return items.Where(m => m.Title.ToLower().Contains(title.ToLower())).ToList();
//    }    

//    public async Task<Diary> GetDiary(int id)
//    {
//        var query = await _indexedDb.Query<Diary>();
//        var item = await query.Where(m => m._Id == id).FirstOrDefaultAsync();
//        item.Content = await _cryptoService.DecryptAsync(item.Content);
//        return item;
//    }

//    public async Task<bool> SaveDiary(Diary diary)
//    {
//        var query = await _indexedDb.Query<Diary>();
//        var enc = await _cryptoService.EncryptAsync(diary.Content);
//        diary.Content = enc.Value;
//        await query.AddAsync(diary);
//        return true;
//    }

//    public async Task<bool> DeleteDiary(int id)
//    {
//        var query = await _indexedDb.Query<Diary>();
//        var exists = await query.Where(m => m._Id == id).FirstOrDefaultAsync();
//        await query.DeleteAsync(exists);
//        return true;
//    }
//}