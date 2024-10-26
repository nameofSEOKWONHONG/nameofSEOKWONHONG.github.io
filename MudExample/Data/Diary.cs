using Blazor.SubtleCrypto;
using Blazored.LocalStorage;

namespace MudExample.Data;

public class DiaryIndex
{
    public string Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}

public class Diary
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string ModifiedBy { get; set; }
}

public class DiaryViewModel
{
    public List<DiaryIndex>? DiaryIndices;
    public List<DiaryIndex>? SelectedDiaryIndexes;
    
    public Diary SelectedDiary { get; set; }
    

    private readonly ILocalStorageService _localStorage;
    private readonly ICryptoService _cryptoService;
    private const string _diaryIndexName = "my_memories_make_me_exist";
    public DiaryViewModel(ILocalStorageService localStorageService, ICryptoService cryptoService)
    {
        _localStorage = localStorageService;
        _cryptoService = cryptoService;
    }

    public async Task Initialize()
    {
        DiaryIndices = await _localStorage.GetItemAsync<List<DiaryIndex>>(_diaryIndexName);        
    }

    public async Task AddDiary(Diary diary)
    {
        var guid = Guid.NewGuid().ToString();
        var date = DateTime.Now;
        var index = new DiaryIndex();
        index.Id = guid;
        index.Title = diary.Title;
        index.CreatedDate = date;
        DiaryIndices.Add(index);
        
        diary.Id = Guid.NewGuid().ToString();
        var enc = await _cryptoService.EncryptAsync(diary.Content);
        diary.Content = enc.Value;
        diary.CreatedDate = date;
        diary.CreatedBy = System.Net.Dns.GetHostName();
        
        await _localStorage.SetItemAsync(_diaryIndexName, DiaryIndices);
        await _localStorage.SetItemAsync(guid, diary);
    }

    public async Task UpdateDiary(Diary diary)
    {
        var date = DateTime.Now;
        diary.Title = diary.Title;
        diary.Content = diary.Content;
        diary.ModifiedDate = DateTime.Now;
        diary.ModifiedBy = System.Net.Dns.GetHostName();
        
        var exist = DiaryIndices.First(m => m.Id == diary.Id);
        exist.ModifiedDate = date;
        
        await _localStorage.SetItemAsync(_diaryIndexName, DiaryIndices);
        await _localStorage.SetItemAsync(diary.Id, diary);
    }

    public async Task GetDiaries()
    {
        var diaries = await _localStorage.GetItemAsync<List<DiaryIndex>>(_diaryIndexName);
        this.DiaryIndices = diaries.OrderByDescending(m => m.CreatedDate).ToList();
    }

    public async Task GetDiary(string id)
    {
        this.SelectedDiary = await _localStorage.GetItemAsync<Diary>(id);
        this.SelectedDiary.Content = await _cryptoService.DecryptAsync(this.SelectedDiary.Content);
    }
}