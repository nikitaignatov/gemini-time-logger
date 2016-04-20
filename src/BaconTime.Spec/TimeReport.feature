Feature: TimeReport
	
Scenario Outline: log time for ticket
	Given I have a ticket
	When I execute log <command>
	Then <total minutes>, <date> and the <comment> is added.
   Examples:
   | command                           | comment        | total minutes | date       |
   | log 1h10m id "Hi mom"             | Hi mom         | 70            | today      |
   | log 10h10m id Hi mom              | Hi mom         | 610           | today      |
   | log 10m id work --when 2016-01-31 | work           | 10            | 2016-01-31 |
   | log 1h id "work"                  | work           | 60            | today      |
   | log 1h id work  --when 2010-02-18 | work           | 60            | 2010-02-18 |
   | log 10h id "work very hard"       | work very hard | 600           | today      |
	
Scenario: show time for ticket
	Given I have a ticket
	And I execute log log 1h10m id --when 2016-01-31 Hi mom 
	And I execute log log 8h30m id --when 2015-01-31 Working Hard 
	When I execute show show logs ticket id
	Then message is shown
	"""
| user         | date       | hours   | message      |
|--------------|------------|---------|--------------|
| Peter Jensen | 2016-01-31 | 1.2     | Hi mom       |
| Peter Jensen | 2015-01-31 | 8.5     | Working Hard |
	"""

Scenario: show all time
	Given I have a ticket
	Given I have another  ticket
	And I execute log log 1h10m id --when 2016-01-31 Hi mom 
	And I execute log log 8h30m id --when 2015-01-31 Working Hard 
	And I execute log log 8h30m another-id --when 2015-01-30 Working Hard 
	When I execute show show time my
	Then message is shown
	"""
| user         | id         | date       | hours   | message      |
|--------------|------------|------------|---------|--------------|
| Peter Jensen | id         | 2016-01-31 | 1.2     | Hi mom       |
| Peter Jensen | id         | 2015-01-31 | 8.5     | Working Hard |
| Peter Jensen | another-id | 2015-01-30 | 8.5     | Working Hard |
	"""