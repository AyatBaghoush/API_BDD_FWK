Feature: SatellitePositions
	

Scenario Outline: Verify that positions can not be provided while the API is missing a timestamp
	Given The user wants to set suppress response code to <suppress_response_code> 
	When Get Satellite API is called for the satellite id = <id> and unit = <unit> and timestamps = <timestamps>
	Then the status code should be <code>
	And the error message should be "invalid timestamp in list: "
	Examples:
	| id    | unit | timestamps | suppress_response_code | code |
	| 25544 |      |            | true                   | 200  |
	| 25544 |      |            |false                   | 400  |
	| 25544 |      |            |                        | 400  |	

Scenario: Verify the API schema and the returned position date
	When Get Satellite API is called for the satellite id = 25544 and unit = kilometers and timestamps = 1436029892
	Then The response should be
	| key        | value           |
	| name       | iss             |
	| id         | 25544           |
	| latitude   | -24.870147579366|
	| longitude  | 17.59950771856  |
	| altitude   | 407.8263965908  |
	| velocity   | 27597.931157627 |
	| visibility | daylight        |
	| footprint  | 4445.0036883392 |
	| timestamp  | 1436029892      |
	| daynum     | 2457208.2163426 |
	| solar_lat  | 22.85465108118  |
	| solar_lon  | 283.22043315343 |
	| units      | kilometers      |


Scenario Outline:  Verify that the number of returned positions corresponds to the number of requested timestamps
	When Get Satellite API is called for the satellite id = <id> and unit = <unit> and timestamps = <timestamps>
	Then the status code should be 200
	And the number of provided positions should be equivalent to requested timestamps <timestamps>
	And The returned units matches with the requested <unit>
	Examples:
	| id    |    unit  | timestamps                         |
	| 25544 |   miles  | 1436029892,1436029902              |
	| 25544 |kilometers|  1436029892,1436029902,1436029992  |


Scenario Outline: Verify that the units is an optional parameter and the default is kilometers
	When Get Satellite API is called for the satellite id = <id> and unit = <unit> and timestamps = <timestamps>
	Then All the returned positions should have "kilometers" as a default unit
	Examples:
	| id    |    unit  | timestamps                         |
	| 25544 |          | 1436029892,1436029902              |
	| 25544 |          |  1436029892,1436029902,1436029992  |


Scenario Outline:  Verify that API can return the satellite position at a past timestamp
	When Get Satellite API is called for the satellite id = <id> at a past timestamp
	Then the status code should be 200
	And the response contains the position of the requested timestamp
	Examples:
	| id    |
	| 25544 |


Scenario Outline:  Verify that API can return the satellite position at a future timestamp
	When Get Satellite API is called for the satellite id = <id> at a future timestamp
	Then the status code should be 200
	And the response contains the position of the requested timestamp
	Examples:
	| id    |
	| 25544 |


Scenario: Verify the API rate limit
	When Get Satellite API is called for number of times that exceeds the rate limit
	Then the status code should be 429
	And the error message should be "Too Many Requests"


Scenario Outline: Verify the number of timestamps is limited to 10
	Given The user wants to set suppress response code to <suppress_response_code>
	When Get Satellite API is called for the satellite id = <id> and unit = <unit> and timestamps = <timestamps>
	Then the status code should be <code>
	Examples:
	| suppress_response_code | code | id | unit | timestamps |
    | true                   | 200 | 25544 |         |1436029892,1436029902,1436029992,1436129892,1436229902,1436329992,1536029892,1636029902,1736029992,1738029992,1738829992|
	|                        | 400 | 25544 |         |1436029892,1436029902,1436029992,1436129892,1436229902,1436329992,1536029892,1636029902,1736029992,1738029992,1738829992|