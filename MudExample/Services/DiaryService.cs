using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using eXtensionSharp;
using MudExample.Data;

namespace MudExample.Services;

public interface IDiaryService
{
    Task<List<DiaryIndex>?> Initialize();
    Task AddDiary(List<DiaryIndex> diaryIndices, Diary diary);
    Task UpdateDiary(List<DiaryIndex> diaryIndices, Diary diary);
    Task<List<DiaryIndex>> GetDiaries();
    Task<Diary> GetDiary(string id);
}

public class DiaryService : IDiaryService
{
    private readonly ILocalStorageService _localStorage;
    private readonly ICryptoService _cryptoService;
    
    private const string _diaryIndexName = "my_memories_make_me_exist";
    
    public DiaryService(ILocalStorageService localStorageService, ICryptoService cryptoService)
    {
        _localStorage = localStorageService;
        _cryptoService = cryptoService;
    }
    
    public async Task<List<DiaryIndex>?> Initialize()
    {
        return await _localStorage.GetItemAsync<List<DiaryIndex>>(_diaryIndexName);        
    }
    
    public async Task AddDiary(List<DiaryIndex> diaryIndices, Diary diary)
    {
        var enc = await _cryptoService.EncryptAsync(diary.Content);
        diary.Content = enc.Value;
        await _localStorage.SetItemAsync(_diaryIndexName, diaryIndices);
        await _localStorage.SetItemAsync(diary.Id, diary);
    }
    
    public async Task UpdateDiary(List<DiaryIndex> diaryIndices, Diary diary)
    {   
        await _localStorage.SetItemAsync(_diaryIndexName, diaryIndices);
        await _localStorage.SetItemAsync(diary.Id, diary);
    }
    
    public async Task<List<DiaryIndex>> GetDiaries()
    {
        var diaries = await _localStorage.GetItemAsync<List<DiaryIndex>>(_diaryIndexName);
        if(diaries.xIsNotEmpty()) return diaries.OrderByDescending(m => m.CreatedDate).ToList();

        return default;
    }
    
    public async Task<Diary> GetDiary(string id)
    {
        var item = await _localStorage.GetItemAsync<Diary>(id);
        item.Content = await _cryptoService.DecryptAsync(item.Content);
        return item; 
    }
}