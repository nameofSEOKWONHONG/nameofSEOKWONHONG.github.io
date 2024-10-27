using eXtensionSharp;
using MudExample.Services;

namespace MudExample.Data;

public class DiaryViewModel
{
    public List<DiaryIndex>? DiaryIndices;
    public HashSet<DiaryIndex>? SelectedDiaryIndexes;
    
    public Diary SelectedDiary { get; set; }

    private readonly IDiaryService _diaryService;
    public DiaryViewModel(IDiaryService diaryService)
    {
        _diaryService = diaryService;
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
        diary.CreatedDate = date;
        diary.CreatedBy = System.Net.Dns.GetHostName();
        
        await _diaryService.AddDiary(this.DiaryIndices, diary);
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

        await _diaryService.UpdateDiary(DiaryIndices, diary);
    }

    public async Task GetDiaries()
    {
        var result = await _diaryService.GetDiaries();
        if (result.xIsEmpty()) result = new();
        
        this.DiaryIndices = result;
    }

    public async Task GetDiary(string id)
    {
        this.SelectedDiary = await _diaryService.GetDiary(id);
    }
}