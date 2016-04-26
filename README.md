# gemini-time-logger

A small cli for logging time with Gemini. This app allows you to create several predefined templates for time logging.

```bat
Usage:
    gemini log <time> <id> [--when=<date>] [--log-type=<type>] <message>... 
    gemini create ticket (<project> <state> <title>...) [--parent=<id>]
    gemini show project all
    gemini show logs
    gemini show logs (project | ticket) <id> [--from=<date>] [--to=<date>]
    gemini show logs user <username> [--from=<date>]  [--to=<date>]
    gemini show ticket (assigned | <id>)
    gemini show hours [by <user>] [--from=<date>] [--to=<date>] [--working-hours=<hours>]  
    gemini show words (all | trend) [--stemmed] [--from=<date>] [--to=<date>] [--user=<id>]
    gemini show stats [--from=<date>] [--to=<date>] [--user=<id>]

Options:
    -h --help                 Show this screen.
    --when=<date>             The date for wen time log entry YYYY-MM-DD [default:now]
    --log-type=<type>         Type of timelogging billable [default:30]
    --from=<date>             The first inclussive date of the time period [default:today-30days]
    --to=<date>               The last inclussive date of the time period [default:today]
    --working-hours=<hours>   The number of working hours in a working day [default:8]
    --stemmed                 Enables porter stemming of the words.
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
"C:\tools\timelogger\Gemini.Commander.exe" %*  
```

Then you can log the time as in the example below

```bat
gemini create ticket 17 1 create time logger cli
gemini log 2h 200 "review existing code"                 
gemini log 3h15m 200 "define test cases and scenarios"                 
gemini log 2h30m 200 "implementation"  --when 2016-04-07                  
gemini log 1h 200 "qa / testing"                 
gemini log 15m 200 "fixing bug"                 
gemini log 10m 200 "deployment"                  
gemini log 5h 200 uat testing  --when 2016-04-08                        
gemini log 10m 200 work        --when 2016-04-10 
gemini log 1h 200 "work"       --when 2016-04-10                  
gemini log 1h 200 work         --when 2010-04-10
gemini log 10h 200 "work very hard"  --when 2016-04-12 
gemini show words      
```

view time logged for a ticket:

```bat
gemini show logs ticket 1024 my

// output:
| date       | hours   | message |
|------------|---------|---------|
| 2016-04-17 | 1.2     | hi      |
```


View your hours by day. To view how many hours missing on the day ```--working-hours=10``` can be used to define working hours length for a day. The default is 8.

```bat
gemini show hours 

// output:
| date          | hours         | missing hours |
|---------------|---------------|---------------|
| 2016-04-24    | 6,6           | 1,4           |
| 2016-04-12    | 10            | -2            |
| 2016-04-10    | 1,2           | 6,8           |
| 2016-04-08    | 5             | 3             |
| 2016-04-07    | 2,5           | 5,5           |
```

view all tickets assigned to you

```bat
gemini show ticket assigned

// output:
| id     | ticket                                              |
|--------|-----------------------------------------------------|
| 36     | Multiple Currency Support                           |
| 78     | Drillable Chart Regions: Wrong Url                  |
| 88     | Support importing from DBF files!                   |
| 70     | Campaign Approvals: Allow Majority Approvals        |
| 189    | Upgrade RestSharp library to RTM                    |
| 197    | Duplication of effort on Helpdesk                   |
| 195    | Month End Accounting takes too long in regions      |
| 176    | Nvarchar max required for comment fields            |
```

