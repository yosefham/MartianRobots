# MartianRobots
Api to help planet exploration using command-driven robots.

This Api provides the ability to manage the exploring of rectangular-shaped planets using command-driven robots.

----
## Inputs
### Planet grid
It's the first line of input. It consists of a pair on integers (x-coordinate, y-coordinate) that represent the upper-right corner, the lower-left coordinates are assumed to be 0, 0.
### Robot Instructions
The remaining input lines are pairs of `initial position` and `movement commands` for each robot.
- **Initial Position**:
It consists of a pair on integers (x-coordinate, y-coordinate) and a letter representing the orientation ( N | W | S | E )
- **Movement Command**:
It consists of consecutive command list ('R' to rotate right, 'L' to rotate left and 'F' to move forward)
>The maximum value for any coordinate is 50 and the maximum number of movement commands is 100. this can be modified in [appsettings.json](https://github.com/yosefham/MartianRobots/blob/master/MartianRobots/appsettings.json)

### Sample Input
```html
["5 3",
"1 1 E",
"RFRFRFRF",
"3 2 N",
"FRRFLLFFRRFLL",
"0 3 W",
"LLFFFRFLFL"]
```
----
## Endpoints
- **MarsExploration/explore**:
  - Process a `List<string>` of instructions to start a planet exploration.
  - Returns the `Id` and some details.
- **MarsExploration/{id}/Update**:
  - Process a `List<string>` of instructions to update a planet exploration. 
  - Returns the `Id` and some details.
  > Planet coordinates are ingored if included.

- **MarsExploration/{id}/Delete**:
  - Delete a planet exploration using `Id`.

- **MarsExploration/{id}/Status**:
  - Get details about a specific planet exploration.

- **MarsExploration/Active**:
  - Get all active Ids.

# Testing
Its possible to run it using IISExpress or Docker, choose your preferred one and lunch it.

Once is running, the easiest way to test it is using swagger UI (navigate to ´/swagger/index.html´), but it's also possible to test it using Postman or a similar software.

----
# Contributions
All contributions and suggestions are welcomed.
