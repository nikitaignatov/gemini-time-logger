Feature: TimeReport
	
Scenario Outline: log time for ticket
	Given I have a ticket
	When I execute log <command>
	Then <total minutes> is and the <comment> is added.
   Examples:
   | command                             | comment        | total minutes |
   | --cmd log -t id -h 1 -m 10 -c "Hi mom"    | Hi mom         | 70            |
   | --cmd log -t id -m 10 -c "work"           | work           | 10            |
   | --cmd log -t id -h 1 -c "work"            | work           | 60            |
   | --cmd log -t id -c work -h 1              | work           | 60            |
   | --cmd log -t id -h 10 -c "work very hard" | work very hard | 600           |
   | --cmd log -t id -m 1 -c  hi               | hi             | 1             |
	
Scenario Outline: show time for ticket
	Given I have a ticket
	And I execute log --cmd log -t id -h 1 -m 10 -c "Hi mom"
	When I execute show <command>
	Then message isshow with <report>
   Examples:
   | command   | report |
   | --cmd show -t id | 1h 10m | 