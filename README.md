# VS - command line utility
Open Visual Studio 2022 in same way as you open VS Code. Pass in a solution file
or a directory, or a period for the current directory if it contains your sln.

## Installation
Use the provided msi installer to install the utility. The installer will add 
the install folder to your *PATH* environment variable. In the future I will
be adding the utility to winget or chocolatey if possible.

You can also clone the solution and build it using Visual Studio. Then you can 
run the installer from the local folder.

```command
git clone git@github.com:elusive/vs.git
cd vs
devenve.exe ./vs.sln /build
```

## Usage
The reason I wanted this utility is that when using git worktrees to easily move
between branches in my projects I needed to be able to switch directories to a 
branch and then open the solution in Visual Studio (2022) without having to grab
the mouse and then browse to the right folder and find the sln file. 

This utility allows me to have a workflow like this, from in one branches folder
I can do something like this:

```bash
git worktree add other-branch
cd ../other-branch
vs .
```

And voila! Visual Studio opens my solution and I can get to work.

## Contributing
Please feel free to enter an issue or offer a suggestion. This is my first try
at something I think might be usable and that I might want to actually publish.
Therefore any feedback would be appreciated.

## License
[MIT](https://choosealicense.com/licenses/mit/)
