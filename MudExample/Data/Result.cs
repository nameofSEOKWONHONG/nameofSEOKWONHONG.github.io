namespace MudExample.Data;

public class Result
{
    public bool Succeed { get; set; }
    public string Message { get; set; }

    public Result()
    {
        
    }
    
    public Result(bool succeed, string message) { this.Succeed = succeed; this.Message = message; } 
}

public class Result<T> : Result
{
    
}