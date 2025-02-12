using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using eXtensionSharp;
using Magic.IndexedDb;
using MudExample.Data;

namespace MudExample.Services;

public interface IDiaryService
{
    Task<List<Diary>> GetDiaries();
    Task<List<Diary>> GetDiaries(string title);
    Task<Diary> GetDiary(int id);
    Task<bool> SaveDiary(Diary diary);
    Task<bool> DeleteDiary(int id);
}

public class DiaryService : IDiaryService
{
    private readonly IMagicDbFactory _magicDbFactory;
    private readonly ICryptoService _cryptoService;
    
    private const string _diaryIndexName = "my_memories_make_me_exist";
    
    public DiaryService(IMagicDbFactory magicDbFactory, ICryptoService cryptoService)
    {
        _magicDbFactory = magicDbFactory;
        _cryptoService = cryptoService;
    }

    public async Task<List<Diary>> GetDiaries()
    {
        var db = await _magicDbFactory.GetDbManagerAsync("MudExampleDb");
        var items = await db.GetAllAsync<Diary>();
        foreach (var diary in items)
        {
            diary.Content = await _cryptoService.DecryptAsync(diary.Content);
        }
        return items.ToList();
    }
    
    public async Task<List<Diary>> GetDiaries(string title)
    {
        var db = await _magicDbFactory.GetDbManagerAsync("MudExampleDb");
        var items = await db.GetAllAsync<Diary>();
        foreach (var diary in items)
        {
            diary.Content = await _cryptoService.DecryptAsync(diary.Content);
        }
        return items.Where(m => m.Title.ToLower().Contains(title.ToLower())).ToList();
    }    

    public async Task<Diary> GetDiary(int id)
    {
        var db = await _magicDbFactory.GetDbManagerAsync("MudExampleDb");
        var item = await db.GetByIdAsync<Diary>(id);
        item.Content = await _cryptoService.DecryptAsync(item.Content);
        return item;
    }

    public async Task<bool> SaveDiary(Diary diary)
    {
        var db = await _magicDbFactory.GetDbManagerAsync("MudExampleDb");
        var enc = await _cryptoService.EncryptAsync(diary.Content);
        diary.Content = enc.Value;
        await db.AddAsync(diary);
        return true;
    }

    public async Task<bool> DeleteDiary(int id)
    {
        var db = await _magicDbFactory.GetDbManagerAsync("MudExampleDb");
        var exists = await db.GetByIdAsync<Diary>(id);
        await db.DeleteAsync(exists);
        return true;
    }
}