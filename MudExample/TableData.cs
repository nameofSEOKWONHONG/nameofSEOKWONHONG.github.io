namespace MudExample;

    public class TableData
    {
        public int TableId { get; set; }
        public string Name { get; set; }
        public List<string> Columns { get; set; }
        public List<Dictionary<string, string>> Data { get; set; }

        public static List<TableData> SeedData()
        {
            var data = new List<TableData>();
            data.Add(new TableData()
            {
                TableId = 1,
                Name = "Series",
                Columns = new List<string>()
                {
                    "Name", "Release year"
                },
                Data = new List<Dictionary<string, string>>(){
                    new Dictionary<string,string>(){
                        ["Name"] = "Game of thrones",
                        ["Realease year"]= "2011",
                    },
                    new Dictionary<string,string>(){
                        ["Name"]="Peaky Blinders",
                        ["Realease year"]= "2013"
                    },
                    new Dictionary<string,string>(){
                        ["Name"]="Breaking Bad",
                        ["Realease year"]= "2008"
                    },
                }
            });

            data.Add(new TableData
            {
                TableId = 2,
                Name = "Films",
                Columns = new List<string>()
                {
                    "Name", "Release year", "Has Oscar"
                },
                Data = new List<Dictionary<string, string>>(){
                    new Dictionary<string,string>(){
                        ["Name"] = "The Green Book",
                        ["Realease year"]= "2018",
                        ["Has Oscar"] = "true"
                    },
                    new Dictionary<string,string>(){
                        ["Name"]="Sherlock",
                        ["Realease year"]= "2009",
                        ["Has Oscar"] = "false"
                    },
                    new Dictionary<string,string>(){
                        ["Name"]="Fight Club",
                        ["Realease year"]= "1999",
                        ["Has Oscar"] = "false",
                    },
                }
            });

            return data;
        }
    }