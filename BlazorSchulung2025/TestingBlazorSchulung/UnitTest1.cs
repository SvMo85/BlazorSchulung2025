using BlazorSchulung2025.Components.Pages;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework.Internal;

namespace TestingBlazorSchulung;

public class Tests : Bunit.TestContext
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestingCounter()
    {
        var cut = RenderComponent<Counter>(); 
        cut.Find("button").Click();
        var theCountValue = cut.Find("p").TextContent;
        
        Assert.That(cut.Instance.CounterValue, Is.EqualTo(1));
        Assert.That(theCountValue, Is.EqualTo("1"));
    }
    
    [Test]
    public void TestingCounterWithStartValue()
    {
        var cut = RenderComponent<Counter>(
            parameters => parameters.Add(p => p.StartCounterAt, 10));
        
        cut.Find("button").Click();
        var theCountValue = cut.Find("p").TextContent;
        
        Assert.That(cut.Instance.CounterValue, Is.EqualTo(11));
    }
    
    
    [Test]
    public void TestingCounterService()
    {
        Services.AddTransient<Microsoft.Extensions.Logging.ILogger<Counter>>(_ => NullLogger<Counter>.Instance);
        
        var cut = RenderComponent<Counter>(
            parameters => parameters.Add(p => p.StartCounterAt, 10));
        
        cut.Find("button").Click();
        var theCountValue = cut.Find("p").TextContent;
        
        Assert.That(cut.Instance.CounterValue, Is.EqualTo(11));
    }


}