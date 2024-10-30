using eXtensionSharp;
using MudExample.Services;

namespace MudExample.Data;

public class DiaryViewModel
{
    public List<Diary>? Diaries;
    public HashSet<Diary>? SelectedDiaries;
    
    public Diary SelectedDiary { get; set; }

    private readonly IDiaryService _diaryService;
    public DiaryViewModel(IDiaryService diaryService)
    {
        _diaryService = diaryService;
    }

    public async Task AddDiary(Diary diary)
    {
        var date = DateTime.Now;
        diary.CreatedDate = date;
        await _diaryService.SaveDiary(diary);
    }

    public async Task GetDiaries()
    {
        var result = await _diaryService.GetDiaries();
        if (result.xIsEmpty()) result = new();
        
        this.Diaries = result;
    }

    public async Task GetDiaries(string title)
    {
        var result = await _diaryService.GetDiaries(title);
        if (result.xIsEmpty()) result = new();
        
        this.Diaries = result;
    }

    public async Task GetDiary(int id)
    {
        this.SelectedDiary = await _diaryService.GetDiary(id);
    }
}