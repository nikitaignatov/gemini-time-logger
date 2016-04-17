Feature: TimeReport
	
Scenario Outline: log time for ticket
	Given I have a ticket
	When I execute log <command>
	Then <total minutes>, <date> and the <comment> is added.
   Examples:
   | command                                       | comment        | total minutes | date       |
   | --cmd log -t id -h 1 -m 10 -c "Hi mom"        | Hi mom         | 70            | today      |
   | --cmd log -t id -m 10 -c "work" -d 2016-01-31 | work           | 10            | 2016-01-31 |
   | --cmd log -t id -h 1 -c "work"                | work           | 60            | today      |
   | --cmd log -t id -c work -h 1 -d 2010-02-18    | work           | 60            | 2010-02-18 |
   | --cmd log -t id -h 10 -c "work very hard"     | work very hard | 600           | today      |
   | --cmd log -t id -m 1 -c  hi                   | hi             | 1             | today      |
	
Scenario: show time for ticket
	Given I have a ticket
	And I execute log --cmd log -t id -h 1 -m 10 -d 2016-01-31 -c "Hi mom" 
	And I execute log --cmd log -t id -h 8 -m 30 -d 2015-01-31 -c "Working Hard" 
	When I execute show --cmd show -t id
	Then message is shown
	"""
+------------+---------+--------------+
| date       | hors    | message      |
+------------+---------+--------------+
| 2016-01-31 | 1.2     | Hi mom       |
+------------+---------+--------------+
| 2015-01-31 | 8.5     | Working Hard |
+------------+---------+--------------+
	"""

Scenario: show all time
	Given I have a ticket
	Given I have another  ticket
	And I execute log --cmd log -t id -h 1 -m 10 -d 2016-01-31 -c "Hi mom" 
	And I execute log --cmd log -t id -h 8 -m 30 -d 2015-01-31 -c "Working Hard" 
	And I execute log --cmd log -t another-id -h 8 -m 30 -d 2015-01-30 -c "Working Hard" 
	When I execute show --cmd show-all
	Then message is shown
	"""
+------------+------------+---------+--------------+
| ticket     | date       | hors    | message      |
+------------+------------+---------+--------------+
| id         | 2016-01-31 | 1.2     | Hi mom       |
+------------+------------+---------+--------------+
| id         | 2015-01-31 | 8.5     | Working Hard |
+------------+------------+---------+--------------+
| another-id | 2015-01-30 | 8.5     | Working Hard |
+------------+------------+---------+--------------+
	"""