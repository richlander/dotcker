using System;
using System.Collections.Generic;
using System.CommandLine;

public class Startup : IStartup<AppCommands>
{
    private RootCommand _command;
    private string _pullString = "--pull";
    private AppCommands _selectedCommand;
    
    public Startup()
    {
        _command = new RootCommand()
        {
            new Option(_pullString){Argument = new Argument(){Arity = ArgumentArity.ZeroOrOne}}
        };
    }

    public (bool success, object argument) Parse(string[] args)
    {
        var error = "no command found";
        var result = _command.Parse(args);

        var pullCommand = result.GetCommandObject(_pullString);

        if (pullCommand is object)
        {
            _selectedCommand = AppCommands.PullImage;
            return (true,pullCommand.Tokens[0]?.Value);
        }
        else if (pullCommand is object && Console.IsInputRedirected)
        {
            _selectedCommand = AppCommands.PullImage;
            return (true,Console.In);
        }

        return (false, error);
    }

    public bool IsCommand(AppCommands command)
    {
        return command == _selectedCommand;
    }

    public AppCommands GetCommand()
    {
        return _selectedCommand;
    }
}