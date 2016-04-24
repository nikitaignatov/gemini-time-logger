# gemini-time-logger

A small cli for logging time with Gemini. This app allows you to create several predefined templates for time logging.

```bat
Usage:
    gemini log <time> <id> [--when=<date>] [--log-type=<type>] <message>... 
    gemini create ticket <project> <state> <title>...
    gemini show logs my
    gemini show logs project <id> [--from=<date>] [--to=<date>]
    gemini show logs ticket  <id> [my] [--from=<date>] [--to=<date>]
    gemini show logs user <username> [--from=<date>]  [--to=<date>]
    gemini show hours my 
    gemini show hours by <user> [--from=<date>]  [--to=<date>]  [--working-hours=<hours>]  
    gemini show words [--stemmed] [--all]

Options:
    -h --help                 Show this screen.
    --when=<date>             The date for wen time log entry YYYY-MM-DD [default:now]
    --log-type=<type>         Type of timelogging billable [default:30]
    --from=<date>             The first inclussive date of the time period [default:today-30days]
    --to=<date>               The last inclussive date of the time period [default:today]
    --working-hours=<hours>   The number of working hours in a working day [default:8]
```

Build the projec, place it in your tools folder. 
in app.config Replace the values with your credentials and the url to Gemini.

```xml
<appSettings>
    <add key="endpoint" value="http://gemini.comany.com" />
    <add key="username" value="USERNAME" />
    <add key="apikey" value="APIKEY" />
</appSettings>
```

Create a ```gemini.bat``` file with a path to where you places the exe.

```bat
@echo off
"C:\tools\timelogger\BaconTime.terminal.exe" %*  
```

Then you can log the time as in the example below

```bat
gemini log 1h10m 1024 "Hi mom"                 
gemini log 10h10m 512 Hi mom                             
gemini log 10m 16 work --when 2016-02-08 
gemini log 1h 32 "work"                  
gemini log 1h 256 work  --when 2010-04-16 
gemini log 10h 1024 "work very hard"       
```

view time logged for a ticket:

```bat
gemini show logs ticket 1024 my

// output:
| date       | hours   | message |
|------------|---------|---------|
| 2016-04-17 | 1.2     | hi      |
```


view all time log entries by you:

```bat
gemini show hours my 

// output:
| user       | ticket               | date                | hours   | message           |
|------------|----------------------|---------------------|---------|-------------------|
```

view your hours by day

```bat
gemini show hours my 

// output:
| date          | hours         | missing hours |
|---------------|---------------|---------------|
```




