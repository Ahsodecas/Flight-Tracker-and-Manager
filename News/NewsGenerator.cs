using Mapsui.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public class NewsGenerator
{
    public List<string> GeneratedNews { get; set; }
    private NewsIterator newsIterator {  get; set; }
    public NewsGenerator(List<INewsProviders> newsProviders, List<IReportable> reportableObjects)
    {
        GeneratedNews = new List<string>();
        newsIterator = new NewsIterator(newsProviders, reportableObjects);
    }
    public void GenerateAllPossibleNews()
    {
        string? newNews = GenerateNextNews();
        while (newNews != null)
        {
            GeneratedNews.Add(newNews);
            newNews = GenerateNextNews();
        }
    }
    public string? GenerateNextNews() 
    {
        Tuple<INewsProviders, IReportable>? newNews = newsIterator.GetNext();
        return (newNews != null) ? (newNews.Item2).Accept(newNews.Item1) : null;
    }
    public void PrintAllNews()
    {
        foreach(var news in  GeneratedNews)
        {
            Console.WriteLine(news);
        }
    }
}

public class NewsIterator
{
    private List<INewsProviders> NewsProviders { get;  set; }
    private List<IReportable> ReportableObjects { get;  set; }
    private int currentProvidersPosition { get; set; }
    private int currentObjectsPosition { get; set; }
    public NewsIterator(List<INewsProviders> newsProviders, List<IReportable> reportableObjects)
    {
        this.NewsProviders = newsProviders;
        this.ReportableObjects = reportableObjects;
        this.currentProvidersPosition = 0; 
        this.currentObjectsPosition = 0;
    }
    public Tuple<INewsProviders, IReportable>? GetNext()
    {
        if (HasMore() == true)
        {
            Tuple<INewsProviders, IReportable> result = new Tuple<INewsProviders, IReportable>(NewsProviders[currentProvidersPosition], ReportableObjects[currentObjectsPosition]);
            if ((this.currentObjectsPosition + 1) < ReportableObjects.Count())
            {
                this.currentObjectsPosition++;
            }
            else if ((this.currentProvidersPosition + 1) < NewsProviders.Count())
            {
                this.currentProvidersPosition++;
                this.currentObjectsPosition = 0;
            }
            else
            {
                return null;
            }
            return result;
        }
        else
        {
            return null;
        }
    }
    public bool HasMore()
    {
        return (this.currentProvidersPosition < NewsProviders.Count()) && (this.currentObjectsPosition < ReportableObjects.Count());
    }

}


