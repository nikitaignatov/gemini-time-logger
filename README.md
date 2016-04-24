# gemini-time-logger


Create a logger.bat file 

```bat
Usage:
    magictimes log <time> <id> [--when=<date>] [--log-type=<type>] <message>... 
    magictimes create ticket (<project> <state> <title>...) [--parent=<id>]
    magictimes show project all
    magictimes show logs
    magictimes show logs (project | ticket) <id> [--from=<date>] [--to=<date>]
    magictimes show logs user <username> [--from=<date>]  [--to=<date>]
    magictimes show ticket (assigned | <id>)
    magictimes show hours [by <user>] [--from=<date>]  [--to=<date>]  [--working-hours=<hours>]  
    magictimes show words [--stemmed] [--all]

Options:
    -h --help                 Show this screen.
    --when=<date>             The date for wen time log entry YYYY-MM-DD [default:now]
    --log-type=<type>         Type of timelogging billable [default:30]
    --from=<date>             The first inclussive date of the time period [default:today-30days]
    --to=<date>               The last inclussive date of the time period [default:today]
    --working-hours=<hours>   The number of working hours in a working day [default:8]
    --stemmed                 Enables porter stemming of the words.
```

Create a ```.bat``` file with a path to your application.

```bat
@echo off
"C:\tools\timelogger\BaconTime.terminal.exe" %*  
```
in app.config Replace the values with your credentials and the url to Gemini.

```xml
<appSettings>
    <add key="endpoint" value="http://gemini.comany.com" />
    <add key="username" value="USERNAME" />
    <add key="apikey" value="APIKEY" />
</appSettings>
```


Then you can log the time as in the example below

```bat
logger log 1h10m 1024 "Hi mom"                 
logger log 10h10m 512 Hi mom                             
logger log 10m 16 work --when 2016-02-08 
logger log 1h 32 "work"                  
logger log 1h 256 work  --when 2010-04-16 
logger log 10h 1024 "work very hard"       
```

view time logged for a ticket:

```bat
logger show -t 1024

// output:
| date       | hours   | message |
|------------|---------|---------|
| 2016-04-17 | 1.2     | hi      |
```


view all time log entries by you:

```bat
logger show-all --my

// output:
| user       | ticket               | date                | hours   | message           |
|------------|----------------------|---------------------|---------|-------------------|
| Peter Jens | Silly ticket         | 17-04-2016 23:00:00 | 1,2     | Worked very hard  |
| Peter Jens | Silly ticket         | 17-04-2016 13:18:45 | 0,8     | hi                |
| Peter Jens | Fix widescreen displ | 16-04-2016 09:39:24 | 5       | commit            |
| Peter Jens | Support importing fr | 14-02-2011 16:30:40 | 0,0     |                   |
| Peter Jens | Drillable Chart Regi | 19-05-2010 09:17:12 | 0,0     | Moe work commited |
| Peter Jens | Webpart Zones: Custo | 30-04-2010 11:23:58 | 1       |                   |
| Peter Jens | Multiple Currency Su | 30-04-2010 11:23:27 | 3       |                   |
| Peter Jens | Multiple Currency Su | 30-04-2010 11:23:08 | 2       |                   |
```


