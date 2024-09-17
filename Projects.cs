using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public static class Projects
{
    public static void SetUpBackgroundWorker(ref BackgroundWorker backgroundWorker, Action action)
    {
        backgroundWorker.WorkerSupportsCancellation = true;
        backgroundWorker.DoWork += (sender, args) => { action(); };
    }

    public static async Task RunInBackgroundWithTimer(TimeSpan timeSpan, Action action)
    {
        var periodicTimer = new PeriodicTimer(timeSpan);
        while (await periodicTimer.WaitForNextTickAsync())
        {
            action();
        }
    }
    public static void Project()
    {
        Data data = new Data();
        FileReader fileReader = new FileReader();
        FileWriter fileWriter = new FileWriter();
        FactoryObjectFromBytes factoryObject = new FactoryObjectFromBytes();
        BackgroundWorker networkSourceReader = new BackgroundWorker();
        BackgroundWorker applicationRunner = new BackgroundWorker();
        BackgroundWorker updateReader = new BackgroundWorker();
        FlightsGUIData dataGUI = new FlightsGUIData();
        Log log = new Log();
        log.StartLogging();
        NetworkSourceSimulator.NetworkSourceSimulator dataSource = new NetworkSourceSimulator.NetworkSourceSimulator("example_data.ftr", 10, 20);
        NetworkSourceSimulator.NetworkSourceSimulator updateSource = new NetworkSourceSimulator.NetworkSourceSimulator("example.ftre", 100, 500);
        Dictionary<string, List<Object>> ReadObjectFTR = fileReader.FTRRead("example_data.ftr", data);
        List<INewsProviders> newsProviders = new List<INewsProviders>() { new Television("Abelian Television"),
                    new Television("Channel TV-Tensor"), new Radio("Quantifier radio"), new Radio("Shmem radio"),
                    new Newspaper("Categories Journal"), new Newspaper("Polytechnical Gazette")};

        fileWriter.JSONWriteManyObjectTypes(ReadObjectFTR);

        dataSource.OnNewDataReady += (sender, args) => { factoryObject.FactoryObject(data, dataSource, args); };

        SetUpBackgroundWorker(ref applicationRunner, FlightTrackerGUI.Runner.Run);
        SetUpBackgroundWorker(ref networkSourceReader, dataSource.Run);
        SetUpBackgroundWorker(ref updateReader, updateSource.Run);

        Subject tempSubject = new Subject();
        tempSubject.AddAllObervers(data.ReadObjects);
        updateSource.OnPositionUpdate += (sender, args) => { tempSubject.NotifyPositionUpdate(args, log); };
        updateSource.OnContactInfoUpdate += (sender, args) => { tempSubject.NotifyContactInfoUpdate(args, log); };
        updateSource.OnIDUpdate += (sender, args) => { tempSubject.NotifyIDUpdate(args, log); };

        applicationRunner.RunWorkerAsync();
        networkSourceReader.RunWorkerAsync();

        _ = RunInBackgroundWithTimer(TimeSpan.FromSeconds(1), () =>
        {
            dataGUI = CreateFlightsGUIData.CreateFlightsGuiData(data.SelectFlights(), data.SelectAirports());
            FlightTrackerGUI.Runner.UpdateGUI(dataGUI);
        });

        updateReader.RunWorkerAsync();
        CommandParser commandParser = new CommandParser(data);

        while (true)
        {
            string? userInstruction = Console.ReadLine();
            if (string.IsNullOrEmpty(userInstruction)) { continue; }
            switch (userInstruction)
            {
                case "print":
                    fileWriter.JSONWriteRandomObjectTypes(data.ReadObjects);
                    break;
                case "exit":
                    log.FinishLogging();
                    if (networkSourceReader.IsBusy)
                        networkSourceReader.CancelAsync();
                    if (updateReader.IsBusy)
                        updateReader.CancelAsync();
                    if (applicationRunner.IsBusy)
                        applicationRunner.CancelAsync();
                    return;
                case "report":
                    List<IReportable> reportableObjects = data.SelectIReportableObjects();   
                    NewsGenerator newsGenerator = new NewsGenerator(newsProviders, reportableObjects);
                    newsGenerator.GenerateAllPossibleNews();
                    newsGenerator.PrintAllNews();
                    break;
                default:
                    commandParser.InvokeCommand(userInstruction);
                    break;
            }
        }  
    }
}
