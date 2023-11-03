# Equa's Branch
#### Last update:
- online
- log system
- better renderer
- world bounds
#### How to connect:
Inside of _GameEngine.cs_ file at 41-st line change __mainUrl_ value on your url
**(url must be the root link to the equa website)**

**Example:**
> string _mainUrl = "https://ce12-80-251-226-62.ngrok-free.app/";

Check if at the end is _/_ symbol

After that open cmd and go to root repository path
**Example:**
> cd /d D:\Files_D\legendary-palm-tree

Type this line to run the project:
> dotnet run

Then cmd ask you to write a nickname
(It souldn't contain *-* or *_* symbols, it's length must be more than 2 symbols and also please don't use special symbols or smth can break)

And that's it!

### Inputs: 
arrows - move,
q - quit the game (don't worry, it can cause an error),
ctrl + c - also quit,

## Bugs we are working on:
- If close/kill/terminate cmd your player object will stay
- There aren't check for used nicknames (can cause an error)
- Not the best render
- Wery slow
- Shooting is turned off for optimization
- No offline mode