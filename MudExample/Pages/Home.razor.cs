using System.Runtime.InteropServices.JavaScript;
using ApexCharts;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudExample.Data;

namespace MudExample.Pages;

public partial class Home
{
    private readonly string[] _dataEnterBarChartXAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    private readonly List<ChartSeries> _dataEnterBarChartSeries = new List<ChartSeries>()
    {
        new ChartSeries
        {
            Name = "Product A",
            Data = new double[] { 1000, 1200, 1500, 1800, 2000, 2300, 2100, 2500, 2400, 2600, 2700, 3000 }
        },
        new ChartSeries
        {
            Name = "Product B",
            Data = new double[] { 800, 1100, 1400, 1700, 1900, 2200, 2000, 2400, 2300, 2500, 2600, 2800 }
        }
    };
    
    public double[] data = { 50, 25, 20, 5 };
    public string[] labels = { "Fossil", "Nuclear", "Solar", "Wind", "Oil", "Coal", "Gas", "Biomass",
        "Hydro", "Geothermal", "Nuclear Fusion", "Pumped Storage", "Solar", "Wind", "Oil",
        "Coal", "Gas", "Biomass", "Hydro", "Geothermal" };
    
    private Position LegendPosition = Position.Bottom;

    public List<ChartSeries> Series = new List<ChartSeries>()
    {
        new ChartSeries() { Name = "United States", Data = new double[] { 40, 20, 25, 27, 46, 60, 48, 80, 15 } },
        new ChartSeries() { Name = "Germany", Data = new double[] { 19, 24, 35, 13, 28, 15, 13, 16, 31 } },
        new ChartSeries() { Name = "Sweden", Data = new double[] { 8, 6, 11, 13, 4, 16, 10, 16, 18 } },
    };
    public string[] XAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep" };

    private List<Order> orders { get; set; } = new List<Order>()
    {
        new Order()
        {
            CustomerName = "John Doe",
            Country = "Korea",
            OrderDate = DateTime.Now,
            OrderType = OrderType.Mail,
            GrossValue = new Random().Next(100000, 1000000),
            DiscountPercentage = 20,
        },
        new Order()
        {
            CustomerName = "Jane Smith",
            Country = "USA",
            OrderDate = DateTime.Now.AddDays(-5),
            OrderType = OrderType.Web,
            GrossValue = new Random().Next(100000, 1000000),
            DiscountPercentage = 10,
        },
        new Order()
        {
            CustomerName = "Alice Johnson",
            Country = "Canada",
            OrderDate = DateTime.Now.AddDays(-10),
            OrderType = OrderType.Phone,
            GrossValue = new Random().Next(100000, 1000000),
            DiscountPercentage = 15,
        },
        new Order()
        {
            CustomerName = "Bob Lee",
            Country = "Australia",
            OrderDate = DateTime.Now.AddDays(-20),
            OrderType = OrderType.Contract,
            GrossValue = new Random().Next(100000, 1000000),
            DiscountPercentage = 5,
        }
    };

    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; }
        public string Country { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public OrderType OrderType { get; set; }
        public decimal GrossValue { get; set; }
        public decimal NetValue { get =>  GrossValue * (1 - (DiscountPercentage / 100)) ; }
        public decimal DiscountPercentage { get; set; }
    }

    public enum OrderType
    {
        Web, Contract, Mail, Phone
    }

    private ApexChartOptions<Order> _options;
    [Inject] LayoutState LayoutState { get; set; }
    protected override void OnInitialized()
    {
        _options = new ApexChartOptions<Order>()
        {
            Theme = new Theme()
            {
                Mode = LayoutState.IsDark ? Mode.Dark : Mode.Light,
                Palette = PaletteType.Palette1,
            }
        };
    }
}