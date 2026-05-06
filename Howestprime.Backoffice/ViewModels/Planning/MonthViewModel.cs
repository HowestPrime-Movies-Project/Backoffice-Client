namespace Howestprime.Backoffice.ViewModels.Planning;

public class MonthViewModel
{
    public DateTime Month { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);

    public string DisplayName { get; set; } = string.Empty;
}
