using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MyHelloWorld.Data;
using MyHelloWorld.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

Console.WriteLine("Hello, World!");

IConfiguration config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();

DataContextDapper myConnection = new DataContextDapper(config);
DataContextEF myEFConnection = new DataContextEF(config);


string testQuery = "SELECT GETDATE()";

DateTime rightNow = myConnection.LoadSingleData<DateTime>(testQuery);

Console.WriteLine(rightNow.ToString());

Computer myPC = new Computer()
{
  Motherboard = "A690",
  CPUCores = 8,
  HasWifi = true,
  HasLTE = false,
  Price = 943.243m,
  VideoCard = "RTX 4060",
  ReleaseDate = DateTime.Now
};

string tableName = "TutorialAppSchema.Computer";

string jsonFile = File.ReadAllText("Computers.json");
string jsonFileSnake = File.ReadAllText("ComputersSnake.json");

JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
{
  PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

Mapper mapper = new Mapper(new MapperConfiguration((cfg) =>
{
  cfg.CreateMap<ComputerSnake, Computer>()
      .ForMember(destination => destination.ComputerId, options =>
          options.MapFrom(source => source.computer_id))
      .ForMember(destination => destination.CPUCores, options =>
          options.MapFrom(source => source.cpu_cores))
      .ForMember(destination => destination.HasLTE, options =>
          options.MapFrom(source => source.has_lte))
      .ForMember(destination => destination.HasWifi, options =>
          options.MapFrom(source => source.has_wifi))
      .ForMember(destination => destination.Motherboard, options =>
          options.MapFrom(source => source.motherboard))
      .ForMember(destination => destination.VideoCard, options =>
          options.MapFrom(source => source.video_card))
      .ForMember(destination => destination.ReleaseDate, options =>
          options.MapFrom(source => source.release_date))
      .ForMember(destination => destination.Price, options =>
          options.MapFrom(source => source.price));
}));

IEnumerable<Computer>? computersNS = JsonConvert.DeserializeObject<IEnumerable<Computer>>(jsonFile);
IEnumerable<Computer>? computersCS = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(jsonFile, jsonOptions);
IEnumerable<ComputerSnake>? computersSnake = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(jsonFileSnake);
IEnumerable<Computer>? computersSnakeAuto = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(jsonFileSnake);

if (computersSnake != null)
{
  IEnumerable<Computer> computerResult = mapper.Map<IEnumerable<Computer>>(computersSnake);
  Console.WriteLine("Automapper Count: " + computerResult.Count());
  // foreach (Computer computer in computerResult)
  // {
  //     Console.WriteLine(computer.Motherboard);
  // }
}

if (computersSnakeAuto != null)
{
  foreach (Computer element in computersSnakeAuto)
  {
    string insertSql = @$"INSERT INTO {tableName}
  (
    Motherboard,
    CPUCores,
    HasWifi,
    HasLTE,
    ReleaseDate,
    Price,
    VideoCard
  ) VALUES (
    '{escapeSingleQuote(element.Motherboard)}',
    '{element.CPUCores}',
    '{element.HasWifi}',
    '{element.HasLTE}',
    '{element.ReleaseDate}',
    '{element.Price}',
    '{escapeSingleQuote(element.VideoCard)}'
  )";
    bool result = myConnection.ExecuteSql(insertSql);

  }
}

string escapeSingleQuote(string param)
{
  return param != null ? param.Replace("'", "''") : "";
}

JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
{
  ContractResolver = new CamelCasePropertyNamesContractResolver()
};

string computersTextNS = JsonConvert.SerializeObject(computersNS, jsonSettings);
File.WriteAllText("computersCopyNS.json", computersTextNS);

string computersTextCS = System.Text.Json.JsonSerializer.Serialize(computersCS, jsonOptions);
File.WriteAllText("computersCopyCS.json", computersTextCS);

string computersTextSnake = System.Text.Json.JsonSerializer.Serialize(computersSnakeAuto);
File.WriteAllText("computersSnakeCopy.json", computersTextSnake);

// string selectSql = @$"SELECT
//   Computer.ComputerId,
//   Computer.Motherboard,
//   Computer.CPUCores,
//   Computer.HasWifi,
//   Computer.HasLTE,
//   Computer.Price,
//   Computer.VideoCard 
// FROM {tableName}";

// IEnumerable<Computer> computers = myConnection.LoadData<Computer>(selectSql);

// Console.WriteLine(@"ComputerId, Motherboard, CPUCores, HasWifi, HasLTE, ReleaseDate, Price, VideoCard");
// foreach (Computer element in computers)
// {
//   Console.WriteLine(@$"'{element.ComputerId}', '{element.Motherboard}', '{element.CPUCores}', '{element.HasWifi}', '{element.HasLTE}', '{element.ReleaseDate.ToString("yyyy-MM-dd")}', '{element.Price}', '{element.VideoCard}'");
// }

IEnumerable<Computer>? computersEF = myEFConnection.Computer?.ToList();

if (computersEF != null)
{
  Console.WriteLine(@"ComputerId, Motherboard, CPUCores, HasWifi, HasLTE, ReleaseDate, Price, VideoCard");
  foreach (Computer element in computersEF)
  {
    Console.WriteLine(@$"'{element.ComputerId}', '{element.Motherboard}', '{element.CPUCores}', '{element.HasWifi}', '{element.HasLTE}', '{element.ReleaseDate}', '{element.Price}', '{element.VideoCard}'");
  }
}
