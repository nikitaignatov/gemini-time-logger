Feature: TimeReport
	
Scenario Outline: log time for ticket
	Given I have a ticket
	When I execute log <command>
	Then <total minutes> is and the <comment> is added.
   Examples:
   | command                             | comment        | total minutes |
   | log -t id -h 1 -m 10 -c "Hi mom"    | Hi mom         | 70            |
   | log -t id -m 10 -c "work"           | work           | 10            |
   | log -t id -h 1 -c "work"            | work           | 60            |
   | log -t id -c work -h 1              | work           | 60            |
   | log -t id -h 10 -c "work very hard" | work very hard | 600           |
   | log -t id -m 1 -c  hi               | hi             | 1             |
	
Scenario Outline: show time for ticket
	Given I have a ticket
	And I execute log log -t id -h 1 -m 10 -c "Hi mom"
	When I execute show <command>
	Then message isshow with <report>
   Examples:
   | command   | report |
   | log -t id | 1h 10m | 