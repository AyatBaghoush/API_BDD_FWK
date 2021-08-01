Feature: Satellite Tles

Scenario Outline: Verify that positions satellites tles could be delivered in different formats and the default is Json
	When the satellite tles is requested for satellite id= <id> with a <format> response 
	Then the status code should be 200
	And the <format> response should contain <response>
Examples:
	| id    | format |  response    |
	| 25544 |        | ISS (ZARYA)  |
	| 25544 | text   | ISS (ZARYA)  |
	| 25544 | json   | ISS (ZARYA)  |


Scenario Outline: Verify the get satellite tles returns an error message in case of wrong id
	Given The user wants to set suppress response code to <suppress_response_code>
	When the satellite tles is requested for satellite id= 25543 with a <format> response 
	Then the status code should be <code>
	And the error message should be "satellite not found"
Examples:
	| format | suppress_response_code | code |
	| text   | true                   | 200  |
	| json   |                        | 404  |
	|        |                        | 404  |


Scenario: Verify the API rate limit
	When Get Satellite tles API is called for number of times that exceeds the rate limit
	Then the status code should be 429
	And the error message should be "Too Many Requests"