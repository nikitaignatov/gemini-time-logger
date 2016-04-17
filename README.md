# gemini-time-logger


Create a logger.bat file 

```cmd
set cli= "C:\tools\timelogger\BaconTime.terminal.exe"
%cli% --endpoint <HTTP://DEMOIU.ONGEMINI.COM> --username <USERNAME> --apikey <APIKEY> --command %*  
```
Replace the params with your credentials and the url to Gemini.

You can also omit the ```--endpoint``` ```--username and``` ```--apikey```

```cmd
"C:\tools\timelogger\BaconTime.terminal.exe"  --command %*  
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
```
