namespace BlazorSchulung2025;

public class LifecycleLogService
{
    private readonly Queue<(string Event, DateTime Timestamp, bool LoggedToConsole)> _eventLog = new();

    // Event für Benachrichtigung über Änderungen
    public event Action? OnLogUpdated;

    public IEnumerable<(string Event, DateTime Timestamp, bool LoggedToConsole)> GetLogEntries() => _eventLog;

    public void LogEvent(string eventName)
    {
        var timestamp = DateTime.Now;
        _eventLog.Enqueue((eventName, timestamp, false));
        OnLogUpdated?.Invoke();
    }

    public void MarkLoggedToConsole(int index)
    {
        if (index < 0 || index >= _eventLog.Count)
            return;

        var eventArray = _eventLog.ToArray();
        var oldEvent = eventArray[index];
        eventArray[index] = (oldEvent.Event, oldEvent.Timestamp, true);

        // Queue neu erstellen
        _eventLog.Clear();
        foreach (var entry in eventArray)
        {
            _eventLog.Enqueue(entry);
        }

        OnLogUpdated?.Invoke();
    }

    public void Clear()
    {
        _eventLog.Clear();
        OnLogUpdated?.Invoke();
    }
}
