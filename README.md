# gemini-time-logger


Create a logger.bat file 

```bat
@echo off
set cli= "C:\tools\timelogger\BaconTime.terminal.exe"
%cli% --endpoint <HTTP://DEMOIU.ONGEMINI.COM> --username <USERNAME> --apikey <APIKEY> --cmd %*  
```
Replace the params with your credentials and the url to Gemini.

You can also omit the ```--endpoint``` ```--username and``` ```--apikey```

```bat
@echo off
"C:\tools\timelogger\BaconTime.terminal.exe"  --cmd %*  
```

and define the values in app.config instead.

```xml
<appSettings>
    <add key="endpoint" value="http://gemini.comany.com" />
    <add key="username" value="USERNAME" />
    <add key="apikey" value="APIKEY" />
</appSettings>
```


Then you can log the time as in the example below

```bat
logger log -t 1024 -h 1    -m 10   -c "Hi mom"    
logger log -t 256  -m 10   -c work                           
logger log -t 512  -h 1    -c work            
logger log -t 16   -c work -h 1              
logger log -t 1024 -h 10   -c "work hard" 
logger log -t 128  -m 1    -c  hi   
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


