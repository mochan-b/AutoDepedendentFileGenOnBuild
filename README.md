# Automating Generating Dependent Files as Part of Build Process in Visual Studio

## Introduction

If there are files that are needed to build the project (e.g. XML files) and those files are generated by an external tool, it can be very annoying to remember to run that tool before every build. We end up wasting a lot of time running the external tool, and we forget to run the tool then we waste even more time with strange behaviors.

We would like to automate the process of running the tool before each build. Obviously, we’d just run the tool and generate the file before running starting the build in a script. It is a little bit tricky in Visual Studio since we’d just like to hit F5 and go into another debugging session. If the tool generates an error, it should also halt the build process and notify of the error.

Here, I’ll go into how to create a project build configuration in Visual Studio so that you can just forget about the pre-build step. When any of the dependent input files to the pre-build step changes, it will run the pre-build script. It will copy the relevant files. If the pre-build steps fails then it will notify as an error rather than silently failing.

## Solution Structure

The pre-build dependent file will be `data.xml` which will be generated by a `XMLData` project and this data.xml file is needed by our main program `XMLMain`. There are two command line executable projects and we will run `XMLData` as part of the build process and copy the files over to `XMLMain` and `XMLDataTest`.

The project structure is that `XMLData` converts `input.txt` to `data.xml` which is used by `XMLMain` to display the XML data. So, if we modify `input.txt`, it should automatically run `XMLData` and generate the `data.xml` and copy it to the `XMLMain` project. If there is an error when creating `data.xml`, the build process should fail and notify the user.

![Solution Structure](img/SolutionOverview.png "Solution Structure")

## Post Build Steps

To run `XMLData` and then copy data.xml to `XMLMain` project directory, the following post build script works.

```bat
$(TargetPath)
copy /y $(TargetDir)data.xml $(SolutionDir)XMLMain
```

This works fine until there is an error in `XMLData` and it won't pick up the error. It will show the exception in the output but thinks the build process went successfully.

![Success Despite Exceptions](img/ExceptionSuccess.png "Success Despite Exceptions")

## Catching Build Errors

The post-build event command line is essentially a batch script. So, we can use batch file commands to detect the return value of `XMLData.exe` and then produce an error if it is not equal to 0.

The next step is to figure out how Visual Studio detects that there are errors in the build process and notifies the user.

1. If the output of a build tool matches a pattern, Visual Studio will show as an error as discussed in the [SOF](https://stackoverflow.com/questions/3704549/visual-studio-post-build-event-throwing-errors) thread and [MSBuild blog](https://blogs.msdn.microsoft.com/msbuild/2006/11/02/msbuild-visual-studio-aware-error-messages-and-message-formats/).
2. If the batch script exits with an error level, the build process will fail with `MSB3073` error and show the set error code.

In batch files, you query the return value of the last program using `%ERRORLEVEL%`. The convention is that if there is no error, then error-level is 0, otherwise is something else defined by the application.

A strange side effect of querying the error-level is that now Visual Studio thinks the post-build step failed with the `MSB3073` error. If you don't query for the error-level, Visual Studio thinks everything is fine. As soon as you query for it, it produces an error. The following code would produce an error when `XMLData` throws an exception.

```bat
$(TargetPath)
if %ERRORLEVEL% == 0 (goto copyxml) ELSE (goto end)

:copyxml
copy /y $(TargetDir)data.xml $(SolutionDir)XMLMain

:end
```

![Build Error MSB3073](img/MSB3073Error.png "Build Error MSB3073")

The exit code of -532462766 is the error level set by `XMLData`.

## Custom Build Error

We’d like to make a little bit nicer build error so that someone building the project know that `XMLData` has failed and it’s not a mysterious error. We will echo out a custom message.

The following code outputs a custom error message.

```bat
$(TargetPath)

if %ERRORLEVEL% == 0 (goto copyxml) ELSE (goto showerror)

:showerror
echo  XMLData.cs : error XMLData001: Exception thrown by XMLData.
goto end

:copyxml
copy /y $(TargetDir)data.xml $(SolutionDir)XMLMain

:end
```

It still throws the MSB3073 error in the error log and I can't seem to get rid of it by resetting the error-level.

However, it does the trick of notifying the user that there is an error with `XMLData` that they should check.

![Better Error](img/BetterError.png "Better Error")

## Final Thoughts

A cooler solution would be remove the `MSB3073` and just show the exception from `XMLData`.

The error message string allows you to specify the file and line number. Since we have only a single file, we can specify that file and if you double click, it will take you to that file. It would also be great to parse out the exception being thrown and print the line number in the error so you can just double click on it and get to the line that caused the exception. For projects with multiple files, it would be great to have the file that caused the exception.

Right now, we just want to make sure build fails when the `XMLData` fails and there is an error message that points to `XMLData` failing.
