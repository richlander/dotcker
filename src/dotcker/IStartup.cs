public interface IStartup<T> where T : System.Enum
{
    (bool success, object argument) Parse(string[] args);
    bool IsCommand(T command);
    T GetCommand();
}